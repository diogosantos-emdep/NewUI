using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Media;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("task_attachments")]
    [DataContract(IsReference =true)]
   public class TaskAttachment : ModelBase,IDisposable
    {
        #region  Fields
        Int64 idTaskAttachment;
        Int64 idTask;
        long? fileSize;
        string fileName;
        string filePath;
        ProjectTask projectTask;
        ImageSource taskAttachmentImage;
        byte[] fileByte;
        #endregion

        #region Properties
        [Key]
        [Column("IdTaskAttachment")]
        [DataMember]
        public Int64 IdTaskAttachment
        {
            get
            {
                return idTaskAttachment;
            }

            set
            {
                idTaskAttachment = value;
                OnPropertyChanged("IdTaskAttachment");
            }
        }

        [Column("IdTask")]
        [ForeignKey("ProjectTask")]
        [DataMember]
        public Int64 IdTask
        {
            get
            {
                return idTask;
            }
            set
            {
                idTask = value;
                OnPropertyChanged("IdTask");
            }
        }

        [Column("FileName")]
        [DataMember]
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        [Column("FilePath")]
        [DataMember]
        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        [Column("FileSize")]
        [DataMember]
        public long? FileSize
        {
            get
            {
                return fileSize;
            }
            set
            {
                fileSize = value;
                OnPropertyChanged("FileSize");
            }
        }

        [NotMapped]
        [DataMember]
        public byte[] FileByte
        {
            get
            {
                return fileByte;
            }
            set
            {
                fileByte = value;
                OnPropertyChanged("FileByte");
            }
        }

        [NotMapped]
        [DataMember]
        public ImageSource TaskAttachmentImage
        {
            get
            {
                return taskAttachmentImage;
            }
            set
            {
                taskAttachmentImage = value;
                OnPropertyChanged("TaskAttachmentImage");
            }
        }

       
        [DataMember]
        public virtual ProjectTask ProjectTask
        {
            get
            {
                return projectTask;
            }
            set
            {
                projectTask = value;
                OnPropertyChanged("ProjectTask");
            }
        }

        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
