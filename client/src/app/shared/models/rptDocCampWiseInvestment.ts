export interface IDocCampWiseInvestment {
    id: number;
    sbuName: string;
    sbuCode: string;
    marketCode: string;
    marketName: string;
    territoryCode: string;
    territoryName: string;
    regionCode: string;
    regionName: string;
    divisionCode: string;
    divisionName: string;
    zoneCode: string;
    zoneName: string;
    institutionId: string;
    institutionName: string;
    doctorId: string;
    doctorName: string;
    doctorCategory: string;
    fromDate: Date | undefined | null;
    toDate: Date | undefined | null;
    investmentPresent: number;
    investmentPast: number;
    brand: string;
    campaign: string;
    subCampaign: string;
    commitment: string;
    actualShareBrand: string;
    actualShare: string;
    competitorShare: string;
    noOfPresc: string;
    noOfPatient: string;
}


