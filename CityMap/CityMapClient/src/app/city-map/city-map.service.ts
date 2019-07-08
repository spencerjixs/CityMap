import { Injectable } from '@angular/core';
import { CityMap } from './city-map.model';
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class CityMapService {
  formData : CityMap;
  readonly rootURL ="http://localhost:56659/api/CityMap/api/CityMap/GetCityMapByZipCode/";
  constructor(private http : HttpClient) { }

  getCityMap(zipCode : string){
    return this.http.get<CityMap>(this.rootURL+zipCode);
  }
}