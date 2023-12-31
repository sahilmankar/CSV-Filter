import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CategorizedFilterProperties } from './filters/CategorizedFilterProperties';
import { FilterRequest } from './filters/filter-request';

@Injectable({
  providedIn: 'root',
})
export class CsvService {
  constructor(private http: HttpClient) {}

  uploadFile(filename: string, formData: FormData): Observable<any> {
    let url = `http://localhost:5237/api/csv/fileupload/${filename}`;
    return this.http.post<any>(url, formData, {
      reportProgress: true,
      observe: 'events',
    });
  }
  getFilterData(
    filterRequest: FilterRequest,
    pageNumber: number = 1
  ): Observable<HttpResponse<any[]>> {
    let url = 'http://localhost:5237/api/csv/filter';
    const params = new HttpParams().set('pageNumber', pageNumber.toString());
    return this.http.post<any[]>(url, filterRequest, {
      params: params,
      observe: 'response',
    });
  }

  processFile(filename: string): Observable<HttpResponse<any[]>> {
    let url = 'http://localhost:5237/api/csv/processfile/' + filename;
    return this.http.get<any[]>(url, {
      observe: 'response'
    });
  }

  getCategorizedProperties(): Observable<CategorizedFilterProperties> {
    let url = 'http://localhost:5237/api/csv/properties';
    return this.http.get<CategorizedFilterProperties>(url);
  }

  getSearchValues(
    property: string,
    searchString: string = ''
  ): Observable<string[]> {
    let url = `http://localhost:5237/api/csv/searching/${property}`;
    const params = new HttpParams().set(
      'searchString',
      searchString.toString()
    );
    return this.http.get<string[]>(url, { params: params });
  }
}
