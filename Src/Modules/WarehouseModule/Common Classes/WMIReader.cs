using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using Emdep.Geos.Data.Common;

namespace WarehouseCommon.Wmi
{
    class WMIReader
    {
        public static IList<ParallelPort> GetPropertyValues(Connection WMIConnection,
                                                      string SelectQuery,
                                                      string className)
        {
            ManagementScope connectionScope = WMIConnection.GetConnectionScope;

            //List<string> alProperties = new List<string>();
            List<ParallelPort> parallelPorts = new List<ParallelPort>();

            SelectQuery msQuery = new SelectQuery(SelectQuery);
            ManagementObjectSearcher searchProcedure = new ManagementObjectSearcher(connectionScope, msQuery);

            try
            {
                foreach (ManagementObject item in searchProcedure.Get())
                {
                    ParallelPort parallelPort = new ParallelPort();

                    foreach (string property in XMLConfig.GetSettings(className))
                    {
                        try
                        {
                            //property + ": " + item[property].ToString()
                            //if (property == "Availability")
                            //{
                            //    parallelPort.Availability = Convert.ToInt64(item[property]);
                            //}

                            switch (property)
                            {
                                case "Availability":
                                    parallelPort.Availability = Convert.ToInt64(item[property]);
                                    break;
                                case "Caption":
                                    parallelPort.Caption = Convert.ToString(item[property]);
                                    break;
                                case "ConfigManagerErrorCode":
                                    parallelPort.ConfigManagerErrorCode = Convert.ToInt64(item[property]);
                                    break;
                                case "ConfigManagerUserConfig":
                                    parallelPort.ConfigManagerUserConfig = Convert.ToBoolean(item[property]);
                                    break;
                                case "CreationClassName":
                                    parallelPort.CreationClassName = Convert.ToString(item[property]);
                                    break;
                                case "Description":
                                    parallelPort.Description = Convert.ToString(item[property]);
                                    break;
                                case "DeviceID":
                                    parallelPort.DeviceID = Convert.ToString(item[property]);
                                    break;
                                case "DMASupport":
                                    parallelPort.DMASupport = Convert.ToBoolean(item[property]);
                                    break;
                                case "Name":
                                    parallelPort.Name = Convert.ToString(item[property]);
                                    break;
                                case "OSAutoDiscovered":
                                    parallelPort.OSAutoDiscovered = Convert.ToBoolean(item[property]);
                                    break;
                                case "PNPDeviceID":
                                    parallelPort.PNPDeviceID = Convert.ToString(item[property]);
                                    break;
                                case "PowerManagementSupported":
                                    parallelPort.PowerManagementSupported = Convert.ToBoolean(item[property]);
                                    break;
                                case "ProtocolSupported":
                                    parallelPort.ProtocolSupported = Convert.ToInt64(item[property]);
                                    break;
                                case "Status":
                                    parallelPort.Status = Convert.ToString(item[property]);
                                    break;
                                case "SystemCreationClassName":
                                    parallelPort.SystemCreationClassName = Convert.ToString(item[property]);
                                    break;
                                case "SystemName":
                                    parallelPort.SystemName = Convert.ToString(item[property]);
                                    break;

                                default:
                                    break;
                            }
                        }
                        catch (SystemException)
                        {
                            /* ignore error */
                        }
                    }

                    parallelPorts.Add(parallelPort);
                }
            }
            catch (ManagementException e)
            {
                /* Do Nothing */
            }

            return parallelPorts;
        }
    }
}
