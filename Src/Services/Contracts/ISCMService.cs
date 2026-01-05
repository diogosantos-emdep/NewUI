
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.SCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Services.Contracts
{
    [ServiceContract]
    public interface ISCMService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetAllCompany_V2490 instead.")]
        List<Data.Common.SCM.Company> GetAllCompany();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetAllFamilies_V2450 instead.")]
        List<Family> GetAllFamilies(string language);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Subfamily> GetAllSubfamilies(string language);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Color> GetAllColors(string language);

        //[rdixit][GEOS2-4399][23.06.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Gender> GetGender(String language);

        //[nsatpute][10-07-2023] SCM - Properties Manager (2/4) GEOS2-4501
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use GetConnectorProperties_V2480 instead.")]
        List<ConnectorProperties> GetConnectorProperties(string language);

        //[Sudhir.Jangra][GEOS2-4502][11/07/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddNewValueKey(ValueKey valueKey, string GeosSettingsKey);


        //[Sudhir.Jangra][GEOS2-4502][18/07/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Language> GetAllLanguages();

        //[Sudhir.Jangra][GEOS2-4501][19/07/2023]
        //[nsatpute][GEOS2-4501][20/07/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use AddPropertyManager_V2460 instead.")]
        void AddPropertyManager(List<ConnectorProperties> connectorProperty);
        //[nsatpute][GEOS2-4501][20/07/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        //[nsatpute][GEOS2-4501][26/07/2023]
        void DeletePropertyManager(List<ConnectorProperties> connectorProperty);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use GetPropertyManager_V2460 instead.")]
        List<ConnectorProperties> GetPropertyManager();
        //[Sudhir.Jangra][GEOS2-4502][19/07/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddCustomProperty(CustomProperty customProperty);

        //[Sudhir.Jangra][GEOS2-4502]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Data.Common.SCM.ValueType> GetValueType();

        //[Sudhir.Jangra][GEOS2-4504]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ValueKey> GetValueKey(string LookupValueKey);

        //[Sudhir.jangra][GEOS2-4504]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateValueKey(ValueKey valueKey);

        //[Sudhir.Jangra][GEOS2-4503]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CustomProperty GetEditCustomProperty(Int32 Id);

        //[Sudhir.Jangra][GEOS2-4505]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateEditCustomProperty(CustomProperty customProperty);
        //[pallavi.jadhav][GEOS2-4562][27/07/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorFamilies> GetConnectorFamilies();

        //[GEOS2-4563][rupali sarode][31-07-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ConnectorFamilies AddConnectorFamilies_V2420(ConnectorFamilies ConnectorFamily, List<Subfamily> SubFamiliesList);
        //[gulab lakade][GEOS2-4506][16 08 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ValueKey> GetAllLookupKey();
        //[gulab lakade][GEOS2-4506][16 08 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookUpValues> GetAllLookUpValuesRecordByIDLookupkey(int IdLookupKey);
        //[gulab lakade][GEOS2-4506][16 08 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertLookupKey_V2420(LookUpValues valueKey);
        //[gulab lakade][GEOS2-4506][16 08 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateLookupKey_V2420(LookUpValues valueKey);
        //[gulab lakade][GEOS2-4506][16 08 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletedValueList_V2420(int IdLookupValue);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ConnectorFamilies UpdateConnectorFamilies_V2420(ConnectorFamilies ConnectorFamily, Int32 IdFamily, List<Subfamily> SubFamiliesList);

        //[Aishwarya Ingale]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Subfamily> GetSubFamily(int IdFamily);

        #region V2430
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use GetAllSubfamilies_V2480 instead.")]
        List<ConnectorSubFamily> GetAllSubfamilies_V2430(string language);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetConnectorFamilies_V2450 instead.")]
        List<ConnectorFamily> GetConnectorFamilies_V2430();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use AddConnectorFamilies_V2450 instead.")]
        ConnectorFamily AddConnectorFamilies_V2430(ConnectorFamily ConnectorFamily);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use UpdateConnectorFamilies_V2450 instead.")]
        ConnectorFamily UpdateConnectorFamilies_V2430(ConnectorFamily ConnectorFamily);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetFamilyImageImagesByIdFamily_V2450 instead.")]
        List<FamilyImage> GetFamilyImageImagesByIdFamily_V2430(int IdFamily, string FamilyName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetSubFamily_V2450 instead.")]
        List<ConnectorSubFamily> GetSubFamily_V2430(int IdFamily, string FamilyName);

        //[GEOS2-4564][rdixit][05.09.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use UpdateSubFamily_V2450 instead.")]
        bool UpdateSubFamily_V2430(ConnectorSubFamily subFamily);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetSubFamilyDetails_V2450 instead.")]
        ConnectorSubFamily GetSubFamilyDetails(int idSubFamily, string familyName);
        #endregion
        //Shubham[skadam] GEOS2-4595 SCM - Search results (1/4) 08 09 2023 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Connectors> GetAllConnectors(Connectors Connectors);
        //Shubham[skadam] GEOS2-4596 SCM - Search results (2/4) 12 09 2023 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use GetAllConnectorImages_V2460 instead.")]
        List<SCMConnectorImage> GetAllConnectorImages(Connectors Connectors);

        //[rdixit][14.09.2023][GEOS2-4602]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ComponentType> GetAllComponentTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CustomProperty> GetAllCustomeData();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use GetAllConnectors_V2460 instead.")]
        List<Connectors> GetAllConnectors_V2440(Connectors Connectors);
        #region [rdixit][GEOS2-4958][20.10.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use GetConnectorFamilies_V2480 instead.")]
        List<ConnectorFamily> GetConnectorFamilies_V2450();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorSubFamily> GetSubFamily_V2450(int IdFamily, string FamilyName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2620. Use GetSubFamilyDetails_V2620 instead.")]
        ConnectorSubFamily GetSubFamilyDetails_V2450(int idSubFamily, string familyName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<FamilyImage> GetFamilyImageImagesByIdFamily_V2450(int IdFamily, string FamilyName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use GetAllFamilies_V2480 instead.")]
        List<Family> GetAllFamilies_V2450(string language);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use AddConnectorFamilies_V2480 instead.")]
        ConnectorFamily AddConnectorFamilies_V2450(ConnectorFamily ConnectorFamily);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2620. Use AddConnectorFamilies_V2620 instead.")]
        bool UpdateSubFamily_V2450(ConnectorSubFamily subFamily);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use UpdateConnectorFamilies_V2480 instead.")]
        ConnectorFamily UpdateConnectorFamilies_V2450(ConnectorFamily ConnectorFamily);

        //[Sudhir.Jangra][GEOS2-4973]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Family> GetFamiliesForSearchFamilyConfiguration_V2460(string language);
        #endregion

        //[rdixit][29.11.2023][GEOS2-4955]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetPropertyManager_V2490 instead.")]
        List<ConnectorProperties> GetPropertyManager_V2460();

        //[rdixit][29.11.2023][GEOS2-4955]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void AddPropertyManager_V2460(List<ConnectorProperties> connectorProperty);

        ////[Sudhir.Jangra][GEOS2-4973]
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //List<Family> GetConfigurationsForSearchFilters_V2460(string language);

        //[rdixit][GEOS2-4951][08.12.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use GetAllConnectors_V2480 instead.")]
        List<Connectors> GetAllConnectors_V2460(Connectors Connectors);

        //[rdixit][GEOS2-4951][08.12.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SCMConnectorImage> GetAllConnectorImages_V2460(Connectors Connectors);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetConfigurationsForSearchFilters_V2490 instead.")]
        List<ConfigurationFamily> GetConfigurationsForSearchFilters_V2460();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use SaveConfigurationsForSearchFilters_V2490 instead.")]
        bool SaveConfigurationsForSearchFilters(List<ConfigurationFamily> ConfigurationList);
        //rajashri GEOS2-4956
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ConnectorFamily GetFamilyDetails_V2470(Int32 idFamily);

        #region [rdixit][GEOS2-5148,5149,5150][29.01.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2620. Use GetConnectorFamilies_V2620 instead.")]
        List<ConnectorFamily> GetConnectorFamilies_V2480();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2620. Use AddConnectorFamilies_V2620 instead.")]
        ConnectorFamily AddConnectorFamilies_V2480(ConnectorFamily ConnectorFamily);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2620. Use UpdateConnectorFamilies_V2620 instead.")]
        ConnectorFamily UpdateConnectorFamilies_V2480(ConnectorFamily ConnectorFamily);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Family> GetAllFamilies_V2480(string language);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorSubFamily> GetAllSubfamilies_V2480(string language);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetAllConnectors_V2490 instead.")]
        List<Connectors> GetAllConnectors_V2480(Connectors Connectors);
        #endregion
        #region //rajashri GEOS2-5227
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorProperties> GetConnectorProperties_V2480(string language);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ValueKey> GetValueKeyOfCustomProperties_V2480(Int32 IdCustomConnectorProperty);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetPropertyManagerByFamily_V2490 instead.")]
        List<ConnectorProperties> GetPropertyManagerByFamily_V2480(UInt32 familyId);
        #endregion
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2580. Use GetAllConnectors_V2580 instead.")]
        List<Connectors> GetAllConnectors_V2490(Connectors Connectors);

        //[Sudhir.Jangra][GEOS2-5204]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Data.Common.Company> GetAuthorizedPlantsByIdUser_V2490(Int32 idUser);

        //[Sudhir.Jangra][GEOS2-5204]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2620. Use GetModifiedSamplesByIdSite_V2620 instead.")]
        List<Samples> GetModifiedSamplesByIdSite_V2490(DateTime startDate, DateTime endDate, Int32 idSite);

        //[Sudhir.Jangra][GEOS2-5205]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ThreeDConnectorItems> Get3dConnectorByIdSite_V2490(DateTime startDate, DateTime endDate, Int32 idSite);

        //[Sudhir.Jangra][GEOS2-5203]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2640. Use GetNewSamplesByIdSite_V2640 instead.")]
        List<Samples> GetNewSamplesByIdSite_V2490(DateTime startDate, DateTime endDate, Int32 idSite);

        //[Sudhir.Jangra][GEOS2-5203]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowStatus> GetStatusForNewSamples_V2490();

        //[GEOS2-5296][rdixit][29.02.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorProperties> GetPropertyManager_V2490();

        //[GEOS2-5297][rdixit][29.02.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorProperties> GetPropertyManagerByFamily_V2490(UInt32 familyId);

        #region [GEOS2-5437][rdixit][07.03.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConfigurationFamily> GetConfigurationsForSearchFilters_V2490();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SimilarColorsByConfiguration> GetSimilarColorsByConfiguration();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use SaveConfigurationsForSearchFilters_V2500 instead.")]
        bool SaveConfigurationsForSearchFilters_V2490(List<ConfigurationFamily> configurationList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddColorsByConfiguration_V2490(List<Tuple<int, int>> colorsList, int idUser);
        #endregion

        //[GEOS2-5296][rdixit][11.03.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Data.Common.SCM.Company> GetAllCompany_V2490();

        //[rdixit][GEOS2-5485][13.03.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool SaveConfigurationsForSearchFilters_V2500(List<ConfigurationFamily> configurationList);

        //[pramod.misal][GEOS2-5378][27-03-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Connectors GetConnectorProperties_V2500(Connectors Connectors);

        //[GEOS2-5297][rdixit][29.02.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorProperties> GetConnectorCustomPropertiesByFamily_V2500(UInt32 familyId);

        //[rdixit][29.03.2024][GEOS2-5380]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SCMConnectorImage> GetAllConnectorImages_V2500(Connectors Connectors);

        //[pramod.misal][GEOS2-5379][29-03-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorReference> GetConnectorReferencesByRef_V2500(string ConnectorRef);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorWorkflowStatus> GetAllConnectorStatus();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorWorkflowTransitions> GetAllWorkflowTransitions();

        //[pramod.misal] [GEOS2-5381] [02.04.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorComponents> GetConnectorComponentsByRef_V2500(string ConnectorRef);

        //[GEOS2-5383][rdixit][11.04.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Connectors> GetAllLinkedConnectorByRef(long idConnector);

        //[pramod.misal] [GEOS2-5387] [09.04.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetConnectorAttachementsByRef_V2520 instead.")]
        List<ConnectorAttachements> GetConnectorAttachementsByRef_V2510(string ConnectorRef);


        //[Sudhir.Jangra][GEOS2-5384]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetAllConnectorImagesForImageSection_V2550 instead.")]
        List<SCMConnectorImage> GetAllConnectorImagesForImageSection_V2510(Connectors Connectors);

        //[GEOS2-5382][rdixit][19.04.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed. Use GetConnectorLocationsByRef_V2550 instead.")]
        List<ConnectorLocation> GetConnectorLocationsByRef(string ConnectorRef);


        //[rdixit][GEOS2-5390][22.04.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorLogEntry> GetConnectorLogEntries(long idConnector);

        //[pramod.misal] [GEOS2-5391] [09.04.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetConnectorCommentsByRef_V2520 instead.")]
        List<ConnectorLogEntry> GetConnectorCommentsByRef_V2510(Int64 IdConnector);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed. Use GetDrawingsByConnectorRef_V2530 instead.")]
        List<ScmDrawing> GetDrawingsByConnectorRef(long idConnector);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticlesbyDrawing> GetArticlesByDrawing(uint idDrawing);

        //[rdixit][02.05.2024][GEOS2-5476]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Data.Common.Company> GetAuthorizedPlants(Int32 idUser);

        //[pramod.misal][GEOS2-5479][30-04-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use UpdateConnectorStatus_V2520 instead.")]
        bool UpdateConnectorStatus_V2510(ConnectorSearch connectorSearch);

        //[rdixit][14.05.2024][GEOS2-5477]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use UpdateConnectorStatus_V2550 instead.")]
        bool UpdateConnectorStatus_V2520(ConnectorSearch connectorSearch);

        //[pramod.misal][15.05.2024]
        [OperationContract]//GetAllConnectorStatus
        [FaultContract(typeof(ServiceException))]
        List<ConnectorAttachements> GetAllConnectorAttachmentTypes();

        //[pramod.misal] [GEOS2-5477] [16.05.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorAttachements> GetConnectorAttachementsByRef_V2520(string ConnectorRef);

        //[rdixit][14.05.2024][GEOS2-5477]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorReference> GetConnectorReferencesByRef_V2520(string ConnectorRef);

        //[rdixit][14.05.2024][GEOS2-5477]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Data.Common.SCM.Company> GetAllCompanyList_V2520();

        //[pramod.misal] [GEOS2-5391] [09.04.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorLogEntry> GetConnectorCommentsByRef_V2520(Int64 IdConnector);

        //[rdiixt][22.05.2024][GEOS2-5751]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorComponents> GetConnectorComponentsByRef_V2520(string ConnectorRef);

        //[rdiixt][22.05.2024][GEOS2-5477]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string CheckOtherRefIsValid(string _ref);

        //[rdiixt][27.05.2024][GEOS2-5753]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Connectors> GetAllLinkedConnectorByRef_V2520(long idConnector);

        //[rdiixt][27.05.2024][GEOS2-5753]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LinkType> GetAllLinkTypes();

        //[rdiixt][27.05.2024][GEOS2-5753]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Connectors CheckLinkedRefIsValid(string _ref);

        //[rushikesh.gaikwad][18.06.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        List<ScmDrawing> GetDrawingsByConnectorRef_V2530(long idConnector);

        //[pramod.misal][GEOS2-5524][05.08.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SCMLocationsManager> GetSCMLocationsManagerByIdSCM_V2550(Data.Common.Company SelectedPlant);

        //[pramod.misal] [GEOS2-5525] [08.08.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 GetMaxPosition(Int64 idParent, Int64 idCompany);

        //[pramod.misal] [GEOS2-5525] [09.08.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        SCMLocationsManager AddSCMLocation(SCMLocationsManager scmLocationsManager);

        //[pramod.misal] [GEOS2-5525] [09.08.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        SCMLocationsManager UpdateSCMLocation(SCMLocationsManager scmLocationsManager);

        //[pramod.misal] [GEOS2-5525] [08.08.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsInUSESCMLocation(Int64 idSampleLocation, Int64 idCompany);

        //[pramod.misal][GEOS2-5524][05.08.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SCMLocationsManager> GetIsLeafSCMLocationsManager_V2550(Data.Common.Company selectedPlant);
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorLocation> GetConnectorLocationsByRef_V2550(string ConnectorRef);

        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2560. Use UpdateConnectorStatus_V2560 instead.")]
        bool UpdateConnectorStatus_V2550(ConnectorSearch connectorSearch);

        //[pramod.misal] [GEOS2-5525] [08.08.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 GetMaxPosition_V2550(Int64 idParent, Int64 idCompany, string fullName);

        //[pramod.misal] [GEOS2-5525] [08.08.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsExistSCMLocationsManagerName(string name, Int64 parent, Int64 idCompany);

        //[rdixit][12.08.2024][GEOS2-5752]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Data.Common.Company> GetAuthorizedPlants_V2550(Int32 idUser);

        //[Rahul.Gadhave][GEOS2-5779][Date-09/08/2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SCMConnectorImage> GetAllConnectorImagesForImageSection_V2550(Connectors Connectors);

        //[pramod.misal][GEOS2-5754][28-08-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2670. Use UpdateConnectorStatus_V2670 instead.")]
        ConnectorSearch UpdateConnectorStatus_V2560(ConnectorSearch connectorSearch);

        #region [rdixit][GEOS2-5802][05.09.2024] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Connectors> GetSampleRegistrationAllConnectors_V2560(Connectors Connectors);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Connectors> GetConnectorsBySearchConfiguration_V2560(Connectors Connectors);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorProperties> GetAllConnectorCustomProperties_V2560();

        //[rushikesh.gaikwad][GEOS2-5801][12.09.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<UserRoles> GetUserRoles_V2560();
        #endregion

        //[GEOS2-5803][rdixit][13.09.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2600. Use GetLatestConnectorReference_V2600 instead.")]
        string GetLatestConnectorReference(string year);

        //[GEOS2-5803][rdixit][13.09.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2650. Use AddConnector_V2650 instead.")]
        bool AddConnector_V2560(ConnectorSearch connectorSearch);

        //[GEOS2-6601][14.11.2024][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Connectors> GetAllConnectors_V2580(Connectors Connectors, string componentQuery);

        //[GEOS2-6080][05.12.2024][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        List<ScmDrawing> GetDrawingsByConnectorRef_V2590(long idConnector);

        //[rdixit][GEOS2-6654][03.01.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        List<Samples> GetReferenceByCustomer_V2600(DateTime startDate, DateTime endDate, string idCustomerList);

        //[rdixit][14.01.2025][GEOS2-6857]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestConnectorReference_V2600(string year);

        //[rdixit][GEOS2-6987][03.03.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2640. Use GetModifiedSamplesByIdSite_V2640 instead.")]
        List<Samples> GetModifiedSamplesByIdSite_V2620(DateTime startDate, DateTime endDate, Int32 idSite);

        //[rdixit][05.03.2025][GEOS2-7026] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateSubFamily_V2620(ConnectorSubFamily subFamily);

        //[rdixit][05.03.2025][GEOS2-7026] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ConnectorFamily AddConnectorFamilies_V2620(ConnectorFamily ConnectorFamily);

        //[rdixit][05.03.2025][GEOS2-7026] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ConnectorFamily UpdateConnectorFamilies_V2620(ConnectorFamily ConnectorFamily);

        //[rdixit][05.03.2025][GEOS2-7026] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorFamily> GetConnectorFamilies_V2620();

        //[rdixit][05.03.2025][GEOS2-7026] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ConnectorSubFamily GetSubFamilyDetails_V2620(int idSubFamily, string familyName);

        //[rdixit][05.03.2025][GEOS2-7026] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorSubFamily> GetSubFamiliesListByIdFamily_V2620(int idFamily);

        //[rdixit][05.03.2025][GEOS2-7026] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<FamilyImage> GetFamilyImagesByIdFamily_V2620(int IdFamily);

        //[rdixit][12.03.2025][GEOS2-7026] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertSubFamily_V2620(ConnectorSubFamily subFamily);

        //[rdixit][GEOS2-6984][27.03.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Connectors> GetAllConnectors_V2630(Connectors Connectors, string componentQuery);

        //[rdixit][GEOS2-7859][14.05.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Samples> GetNewSamplesByIdSite_V2640(DateTime startDate, DateTime endDate, Int32 idSite);

        #region [GEOS2-7863][rani dhamankar][12-05-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ThreeDConnectorItems> Get3dConnectorByIdSite_V2640(DateTime startDate, DateTime endDate, Int32 idSite);
        #endregion

        //[rdixit][GEOS2-7861][14.05.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Samples> GetModifiedSamplesByIdSite_V2640(DateTime startDate, DateTime endDate, Int32 idSite);

        #region [GEOS2-7855][rani dhamankar][15-05-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorProperties> GetPropertyManagerByFamily_V2640(string familyId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2650. Use GetAllConnectors_V2650 instead.")]
        List<Connectors> GetAllConnectors_V2640(Connectors Connectors, string componentQuery, bool getAllRecords);
        #endregion

        //[rdixit][GEOS2-8133][26.05.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Connectors> GetSampleRegistrationAllConnectors_V2640(Connectors Connectors, bool isLoadAll);

        //[GEOS2-8088][rdixit][29.05.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2680. Use AddConnector_V2680 instead.")]
        string AddConnector_V2650(ConnectorSearch connectorSearch);

        //[rdixit][11.06.2025][GEOS2-6644]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool SaveConfigurationsForSearchFilters_V2650(List<ConfigurationFamily> configurationList);

        //[rdixit][11.06.2025][GEOS2-6644]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SimilarCharactersByConfiguration> GetSimilarCharachtersByConfiguration();

        //[rdixit][19.06.2025][GEOS2-6645]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Connectors> GetAllConnectors_V2650(Connectors Connectors, string componentQuery, bool getAllRecords);

        //[nsatpute][16.07.2025][GEOS2-8090]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsConnectorExists(string connectorRerence);

        //[nsatpute][22.07.2025][GEOS2-8090]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ScmDrawing> GetDrawingsByCustomerRef_V2660(string customerRef);

        //[nsatpute][22.07.2025][GEOS2-8090]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Emdep.Geos.Data.Common.Company> GetAllCompaniesWithServiceProvider_V2660();

        //[rdixit][GEOS2-9199][11.09.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ConnectorSearch UpdateConnectorStatus_V2670(ConnectorSearch connectorSearch);

        //[rdixit][18.09.2025][GEOS2-]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Connectors GetConnectorProperties_V2670(Connectors Connectors);

        //[rdixit][18.09.2025][GEOS2-]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorProperties> GetCustomeProperties_V2670();

        //[rdixit][01.10.2025][GEOS2-9552]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2670. Use GetAllConnectors_V2680 instead.")]
        List<Connectors> GetAllConnectors_V2670(Connectors Connectors, string componentQuery, bool getAllRecords);

        //[rdixit][14.10.2025][GEOS2-8895]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string AddConnector_V2680(ConnectorSearch connectorSearch);
        //[Rahul.Gadhave][GEOS2-9556][Date:04/11/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteReference_V2680(string Reference,Int64 IdConnector, Int32 IdUser);
        //[Rahul.Gadhave][GEOS2-9556][Date:04/11/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool GetConnectorCreatorEmailAndSendMail_V2680(Int64 IdConnector);
        //[Rahul.Gadhave][GEOS2-9556][Date:05/11/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Connectors> GetAllConnectors_V2680(Connectors Connectors, string componentQuery, bool getAllRecords);

    }
}
