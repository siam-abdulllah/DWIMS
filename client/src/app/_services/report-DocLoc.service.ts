import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';

@Injectable({
  providedIn: 'root'
})
export class RptDocLocService {
  
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();
  constructor(private http: HttpClient, private router: Router) { }
 
  getDoctors() {
    return this.http.get(this.baseUrl + 'doctor/doctorsForReport');
  }

  GetDocLocMapInd(doctorName:any,doctorCode:any, marketName: string, marketCode: string){ 
    debugger;
    if(doctorCode==undefined || doctorCode=="")
    {
      doctorCode=0;
    }
    if(doctorName==undefined || doctorName=="")
    {
      doctorName="undefined";
    }
    if(marketName==undefined || marketName=="")
    {
      marketName="undefined";
    }
    if(marketCode==undefined || marketCode=="")
    {
      marketCode="undefined";
    }
    return this.http.get(this.baseUrl+ 'DoctorLocation/getDocLocMapInd/'+doctorName+'/'+doctorCode+'/'+marketName+'/'+marketCode);  
  }

  // IsInvestmentInActive(referenceNo: string){ 
  //   return this.http.get(this.baseUrl+ 'reportInvestment/isInvestmentInActive/'+referenceNo);  
  // }
  // IsInvestmentInActiveDoc(referenceNo: string,doctorId:number,doctorName:string){ 
  //   return this.http.get(this.baseUrl+ 'reportInvestment/IsInvestmentInActiveDoc/'+referenceNo+'/'+doctorId+'/'+doctorName);  
  // }

  getRptDepotLetter(initId:any) {
    return this.http.get(this.baseUrl+ 'reportInvestment/rptInvestDepo/'+initId);
  }

}

