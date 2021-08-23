
export interface IMarket {
    marketName: string;
    marketCode: string;
}
 
export class Market implements IMarket {
    marketName: string;
    marketCode: string;
}

export interface IDivision {
    divisionName: string;
    divisionCode: string;
}
 
export class Division implements IDivision {
    divisionName: string;
    divisionCode: string;
}

export interface IZone {
    zoneName: string;
    zoneCode: string;
}
 
export class Zone implements IZone {
    zoneName: string;
    zoneCode: string;
}

export interface ITerritory {
    territoryName: string;
    territoryCode: string;
}
 
export class Territory implements ITerritory {
    territoryName: string;
    territoryCode: string;
}

export interface IRegion {
    regionName: string;
    regionCode: string;
}
 
export class Region implements IRegion {
    regionName: string;
    regionCode: string;
}