using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.DataAccess;
using Emdep.Geos.Data.Common;
using System.Net;
using System.Data.Entity;



namespace Emdep.Geos.Data.BusinessLogic
{
    public class GeosWorkbenchVersionsManager
    {
        /// <summary>
        /// This method is to get current version from class GeosWorkbenchVersion
        /// </summary>
        /// <returns>Details of current version from class GeosWorkbenchVersion</returns>
        public GeosWorkbenchVersion GetCurrentVersion()
        {
            GeosWorkbenchVersion geosworkbenchversion = null;
            using (var db = new WorkbenchContext())
            {
                geosworkbenchversion = (from records in db.GeosWorkbenchVersions
                                        orderby records.IdGeosWorkbenchVersion descending
                                        select records).FirstOrDefault();

            }
            return geosworkbenchversion;
        }

        public GeosWorkbenchVersion GetCurrentVersionBetaWise(Int32 userId)
        {
            GeosWorkbenchVersion geosworkbenchversion = null;
            GeosVersionBetaTester isCheckBetaTester = null;
            using (var db = new WorkbenchContext())
            {
                isCheckBetaTester = (db.GeosVersionBetaTesters.Where(userRecord => userRecord.IdUser == userId).OrderByDescending(obd => obd.IdGeosVersion).FirstOrDefault());
                if (isCheckBetaTester == null)
                    geosworkbenchversion = (from records in db.GeosWorkbenchVersions
                                            where records.IsBeta == 0
                                            orderby records.IdGeosWorkbenchVersion descending
                                            select records).FirstOrDefault();
                else
                    geosworkbenchversion = (from records in db.GeosWorkbenchVersions
                                            where records.IdGeosWorkbenchVersion == isCheckBetaTester.IdGeosVersion
                                            select records).FirstOrDefault();

            }
            return geosworkbenchversion;
        }

        public GeosWorkbenchVersion GetUserIsBetaCurrentVersion(Int32 userId)
        {
            GeosWorkbenchVersion geosworkbenchversion = null;
            GeosVersionBetaTester isCheckBetaTester = null;
            using (var db = new WorkbenchContext())
            {
                isCheckBetaTester = (db.GeosVersionBetaTesters.Where(userRecord => userRecord.IdUser == userId).OrderByDescending(obd => obd.IdGeosVersion).FirstOrDefault());
                if (isCheckBetaTester == null)
                    geosworkbenchversion = null;
                else
                    geosworkbenchversion = (from records in db.GeosWorkbenchVersions
                                            where records.IdGeosWorkbenchVersion == isCheckBetaTester.IdGeosVersion
                                            select records).FirstOrDefault();

            }
            return geosworkbenchversion;
        }


        public List<GeosWorkbenchVersion> GetAllVersionBetaWise(Int32 userId)
        {
            List<GeosWorkbenchVersion> geosworkbenchversion = null;
            GeosVersionBetaTester isCheckBetaTester = null;
            using (var db = new WorkbenchContext())
            {
                isCheckBetaTester = (db.GeosVersionBetaTesters.Where(userRecord => userRecord.IdUser == userId).OrderByDescending(obd => obd.IdGeosVersion).FirstOrDefault());
                if (isCheckBetaTester == null)
                    geosworkbenchversion = (from records in db.GeosWorkbenchVersions
                                            where records.IsBeta == 0
                                            orderby records.IdGeosWorkbenchVersion descending
                                            select records).ToList();
                else
                    geosworkbenchversion = (from records in db.GeosWorkbenchVersions
                                            where records.IdGeosWorkbenchVersion == isCheckBetaTester.IdGeosVersion
                                            select records).ToList();

            }
            return geosworkbenchversion;
        }



        public GeosWorkbenchVersion GetCurrentPublishVersion()
        {
            GeosWorkbenchVersion geosworkbenchversion = null;
            using (var db = new WorkbenchContext())
            {
                geosworkbenchversion = (from records in db.GeosWorkbenchVersions
                                        where records.IsPublish == 1
                                        orderby records.IdGeosWorkbenchVersion descending
                                        select records).FirstOrDefault();

            }
            return geosworkbenchversion;
        }

        /// <summary>
        /// This method is to get all versions from class GeosWorkbenchVersion
        /// </summary>
        /// <returns>List of GeosWorkbenchVersion</returns>
        public List<GeosWorkbenchVersion> GetAllVersions()
        {
            List<GeosWorkbenchVersion> geosworkbenchversion = null;
            using (var db = new WorkbenchContext())
            {
                geosworkbenchversion = (from records in db.GeosWorkbenchVersions
                                        select records).ToList();

            }
            return geosworkbenchversion;
        }

        /// <summary>
        /// This method is to get current version release notes from class GeosReleaseNote
        /// </summary>
        /// <param name="geosworkbenchversion">To get details of current version from class GeosWorkbenchVersion</param>
        /// <returns>List of GeosReleaseNotes related to current version</returns>
        public List<GeosReleaseNote> GetReleaseNotesByVersion(GeosWorkbenchVersion geosWorkbenchVersion)
        {
            List<GeosReleaseNote> geosreleasenote = null;
            using (var db = new WorkbenchContext())
            {
                geosreleasenote = (from records in db.GeosReleaseNotes.Include("GeosModule")
                                   where records.IdGeosWorkbenchVersion == geosWorkbenchVersion.IdGeosWorkbenchVersion
                                   select records).ToList();
            }
            return geosreleasenote;
        }

        /// <summary>
        /// This method is to add download version by IP in class GeosWorkbenchVersionDownload
        /// </summary>
        /// <param name="geosworkbenchversion">To get details of current version from class GeosWorkbenchVersion</param>
        public void AddDownloadVersionByIP(GeosWorkbenchVersionDownload geosWorkbenchVersionDownload)
        {
            using (var db = new WorkbenchContext())
            {
                db.GeosWorkbenchVersionDownload.Add(geosWorkbenchVersionDownload);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// This method is to get current version files from class GeosWorkbenchVersionsFile
        /// </summary>
        /// <param name="IdGeosWorkbenchVersion">Get current version id from class GeosWorkbenchVersion</param>
        /// <returns>List of GeosWorkbenchVersionsFiles related to current version id</returns>
        public List<GeosWorkbenchVersionsFile> GetWorkbenchVersionFiles(Int32 idGeosWorkbenchVersion)
        {
            List<GeosWorkbenchVersionsFile> geosworkbenchversionsfile = null;
            using (var db = new WorkbenchContext())
            {
                geosworkbenchversionsfile = (from records in db.GeosWorkbenchVersionsFiles
                                             where records.IdGeosWorkbenchVersion == idGeosWorkbenchVersion
                                             select records).ToList();
            }
            return geosworkbenchversionsfile;
        }

        /// <summary>
        /// This method is to get list of documentations related to current version id from class GeosModuleDocumentation
        /// </summary>
        /// <param name="IdGeosWorkbenchVersion">Get current version id from class GeosWorkbenchVersion</param>
        /// <returns>List of GeosModuleDocumentations related to current version id</returns>
        public List<GeosModuleDocumentation> GetModuleDocumentations(Int32 idGeosWorkbenchVersion)
        {
            List<GeosModuleDocumentation> geosmoduledocumentation = null;
            using (var db = new WorkbenchContext())
            {
                geosmoduledocumentation = (from records in db.GeosModuleDocumentations
                                           where records.IdGeosWorkbenchVersion == idGeosWorkbenchVersion
                                           select records).ToList();
            }
            return geosmoduledocumentation;
        }

        /// <summary>
        /// This method is to get document filename related to document id from class GeosModuleDocumentation
        /// </summary>
        /// <param name="IdGeosModuleDocumentation">Get current version document id from class GeosModuleDocumentation</param>
        /// <returns>FileName of related version document id</returns>
        public String GetModuleDocumentFileName(Int32 idGeosModuleDocumentation)
        {
            String fileName = null;
            using (var db = new WorkbenchContext())
            {
                fileName = (from records in db.GeosModuleDocumentations
                            where records.IdGeosModuleDocumentation == idGeosModuleDocumentation
                            select records).Select(a => a.FileName).SingleOrDefault();
            }
            return fileName;
        }

        /// <summary>
        /// This method is to get list of modules related to user id permission
        /// </summary>
        /// <param name="idUser">Get id user</param>
        /// <returns>List of modules related to user id permission</returns>
        public List<GeosModule> GetUserModulesPermissions(int idUser)
        {
            List<GeosModule> geosModules = null;
            using (var db = new WorkbenchContext())
            {
                List<Int32> notIdPermissions = new List<Int32>();
                notIdPermissions.Add(20);
                notIdPermissions.Add(21);
                notIdPermissions.Add(22);
                geosModules = (from records in db.GeosModule
                               join permission in db.Permissions on records.IdGeosModule equals permission.IdGeosModule
                               join userpermission in db.UserPermissions on permission.IdPermission equals userpermission.IdPermission
                               where userpermission.IdUser == idUser && !notIdPermissions.Contains(permission.IdPermission)
                               select records
                               ).Include("UIModuleThemes.UITheme").ToList();
            }

            return geosModules;
        }

        /// <summary>
        /// This method is to get all workbench version
        /// </summary>
        /// <returns>List of all geos workbench version</returns>
        public List<GeosWorkbenchVersion> GetAllWorkbenchVersion()
        {
            List<GeosWorkbenchVersion> geosAllWorkbenchVersion = null;
            using (var db = new WorkbenchContext())
            {
                geosAllWorkbenchVersion = (from records in db.GeosWorkbenchVersions.Include("GeosReleaseNotes.GeosModule") select records).ToList();
            }
            return geosAllWorkbenchVersion;
        }

        /// <summary>
        /// This method is to get workbench version by id
        /// </summary>
        /// <param name="idGeosWorkbenchVersion">Get id of workbench version</param>
        /// <returns>Details of workbench version by id</returns>
        public GeosWorkbenchVersion GetWorkbenchVersionById(Int32 idGeosWorkbenchVersion)
        {
            GeosWorkbenchVersion geosWorkbenchVersion = null;
            using (var db = new WorkbenchContext())
            {
                geosWorkbenchVersion = (from records in db.GeosWorkbenchVersions
                                        where records.IdGeosWorkbenchVersion == idGeosWorkbenchVersion
                                        select records).SingleOrDefault();
            }
            return geosWorkbenchVersion;
        }

        /// <summary>
        /// This method is to get back version of geos workbench
        /// </summary>
        /// <param name="idGeosWorkbenchVersion">Get latest version id</param>
        /// <returns>Get back version of geos workbench</returns>
        public GeosWorkbenchVersion GetWorkbenchBackVersionToRestoreById(Int32 idGeosWorkbenchVersion)
        {
            GeosWorkbenchVersion geosWorkbenchVersion = null;
            using (var db = new WorkbenchContext())
            {
                geosWorkbenchVersion = (from records in db.GeosWorkbenchVersions
                                        orderby records.IdGeosWorkbenchVersion descending
                                        where records.IdGeosWorkbenchVersion != idGeosWorkbenchVersion
                                        select records).FirstOrDefault();
            }
            return geosWorkbenchVersion;
        }

        /// <summary>
        /// This method is to get workbench version by version number
        /// </summary>
        /// <param name="idGeosWorkbenchVersion">Get version number of workbench version</param>
        /// <returns>Details of workbench version by version number</returns>
        public GeosWorkbenchVersion GetWorkbenchVersionByVersionNumber(string versionNumber)
        {
            GeosWorkbenchVersion geosWorkbenchVersion = null;
            using (var db = new WorkbenchContext())
            {
                geosWorkbenchVersion = (from records in db.GeosWorkbenchVersions
                                        orderby records.IdGeosWorkbenchVersion descending
                                        where records.VersionNumber == versionNumber
                                        select records).FirstOrDefault();
            }
            return geosWorkbenchVersion;
        }

        /// <summary>
        /// This method is to get Geos provider details
        /// </summary>
        /// <param name="idCompany">Get company id</param>
        /// <returns>Geos provider details</returns>
        public GeosProvider GetServiceProviderDetailByCompanyId(Int32 idCompany)
        {
            GeosProvider geosProvider = null;
            using (var db = new WorkbenchContext())
            {
                geosProvider = (from records in db.GeosProviders
                                where records.IdCompany == idCompany
                                select records).SingleOrDefault();
            }
            return geosProvider;
        }
    }
}
