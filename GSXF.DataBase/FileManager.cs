using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using GSXF.Model;
using GSXF.Auxiliary;
using System.Web;




namespace GSXF.DataBase
{
    public class FileManager : BaseManager<UploadFile>
    {
        public override Response Delete(int ID)
        {
            UploadFile file = Find(ID);
            string filePath = HttpContext.Current.Server.MapPath(file.Path);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return base.Delete(ID);
        }

    }
}
