export interface IInsSocBcdsInvestment {
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
    societyId: string;
    societyName: string;
    bcdsId: string;
    bcdsName: string;
    donationType: string;
    fromDate: Date | undefined | null;
    toDate: Date | undefined | null;
    investedAmt: number;
    commitment: string;
    actualShare: string;
    competitorShare: string;
}