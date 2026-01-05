using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.APM.CommonClasses
{
    public class ImageCacheService
    {
        private static readonly Lazy<ImageCacheService> _instance = new Lazy<ImageCacheService>(() => new ImageCacheService());
        public static ImageCacheService Instance => _instance.Value;
        private readonly ConcurrentDictionary<string, ImageSource> _memoryCache = new ConcurrentDictionary<string, ImageSource>();
        private readonly HttpClient _httpClient;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new ConcurrentDictionary<string, SemaphoreSlim>();
        private readonly string _cacheFolder;

        private readonly string _logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GEOS_Performance_Logs", "ImageDownloadLog.txt");
        private readonly object _logLock = new object();

        private ImageCacheService()
        {
            var handler = new HttpClientHandler
            {
                UseDefaultCredentials = true
            };

            _httpClient = new HttpClient(handler);
            _cacheFolder = Path.Combine(Path.GetTempPath(), "GeosImageCache");
            if (!Directory.Exists(_cacheFolder))
            {
                Directory.CreateDirectory(_cacheFolder);
            }

            var logDir = Path.GetDirectoryName(_logPath);
            if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
        }



        public async Task<ImageSource> GetImageAsync(string employeeCode, string urlRounded, string urlNormal)
        {
            if (string.IsNullOrEmpty(employeeCode)) return null;
            string cacheKey = $"emp_{employeeCode}";

            if (_memoryCache.TryGetValue(cacheKey, out var memoryImage)) return memoryImage;

            var myLock = _locks.GetOrAdd(cacheKey, k => new SemaphoreSlim(1, 1));
            await myLock.WaitAsync();

            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out memoryImage)) return memoryImage;

                string localPath = Path.Combine(_cacheFolder, $"{employeeCode}.png");

                if (File.Exists(localPath))
                {
                    var diskImage = await Task.Run(() => LoadFromBytes(File.ReadAllBytes(localPath)));
                    if (diskImage != null)
                    {
                        _memoryCache[cacheKey] = diskImage;
                        return diskImage;
                    }
                }

                byte[] imageBytes = null;

                try
                {
                    imageBytes = await _httpClient.GetByteArrayAsync(urlRounded);
                }
                catch {  }

                if (imageBytes == null || imageBytes.Length == 0)
                {
                    try
                    {
                        imageBytes = await _httpClient.GetByteArrayAsync(urlNormal);
                    }
                    catch {  }
                }

                if (imageBytes != null && imageBytes.Length > 0)
                {
                    using (var stream = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                    {
                        await stream.WriteAsync(imageBytes, 0, imageBytes.Length);
                    }

                    var webImage = await Task.Run(() => LoadFromBytes(imageBytes));
                    if (webImage != null) _memoryCache[cacheKey] = webImage;
                    return webImage;
                }

                return null;
            }
            finally
            {
                myLock.Release();
            }
        }

        private ImageSource LoadFromBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            try
            {
                var bitmap = new BitmapImage();
                using (var stream = new MemoryStream(bytes))
                {
                    bitmap.BeginInit();
                    bitmap.DecodePixelWidth = 64; 
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    bitmap.Freeze();
                }
                return bitmap;
            }
            catch { return null; }
        }
    }
}