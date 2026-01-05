using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Emdep.Geos.Data.Common;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Data;
namespace Emdep.Geos.UI.ServiceProcess
{
    public class CrmRestServiceController
    {
        //public static string serviceUrl = "";
        static string serviceAddress = "";
        public CrmRestServiceController(string serviceUrl)
        {
            serviceAddress = "http://" + serviceUrl + "/CrmRestService.svc";
        }

        public ObservableCollection<PeopleDetails> GetPeoples()
        {
            ObservableCollection<PeopleDetails> Peoples = new ObservableCollection<PeopleDetails>();
            string path = serviceAddress + "/GetPeoples";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(path);
            request.Method = "POST";
            request.ContentType = "application/json";

            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(string));
            json.WriteObject(request.GetRequestStream(), "");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();

            DataContractJsonSerializer jsonResp = new DataContractJsonSerializer(typeof(ObservableCollection<PeopleDetails>));

            Peoples = (ObservableCollection<PeopleDetails>)jsonResp.ReadObject(stream);


            stream.Flush();
            stream.Close();
            return Peoples;
        }
    }
}
