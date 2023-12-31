import { Component } from '@angular/core';
import { CategorizedFilterProperties } from '../filters/CategorizedFilterProperties';
import { EqualPropertiesDataSource } from '../filters/EqualPropertiesDataSource';
import { FilterRequest } from '../filters/filter-request';
import { FilterService } from '../filters/filter.service';
import { PaginationHeader } from '../filters/paginationHeader';
import { CsvService } from '../csv.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-csv-main',
  templateUrl: './csv-main.component.html',
  styleUrls: ['./csv-main.component.css'],
})
export class CsvMainComponent {
  data: any[] = [];

  filterRequest: FilterRequest = {
    equalFilters: [],
    rangeFilters: [],
    dateRangeFilters: [],
    sortBy: undefined,
    searchString: undefined,
    sortAscending: false,
  };

  paginationData: PaginationHeader | null = null;

  categorizedProperties!: CategorizedFilterProperties;

  equalPropertiesDataSources: EqualPropertiesDataSource[] = [];

  constructor(
    private csvSvc: CsvService,
    private filtersvc: FilterService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      let filename = params.get('file');
      if (filename) this.onFileUpload(filename);
    });
  }

  getFilterData(pageInfo:any) {
    let pageNumber=pageInfo.page;
    let pageSize=pageInfo.pageSize;
    let newfilterreq = this.filtersvc.removeDefaultFilterValues(
      this.filterRequest
    );
    this.csvSvc.getFilterData(newfilterreq, pageNumber,pageSize).subscribe((res) => {
      this.data = res.body ?? [];
      this.paginationData = this.filtersvc.getPaginationHeader(res);
    });
  }

  onFileUpload(filename: string) {
    this.csvSvc.processFile(filename).subscribe((res) => {
      this.data = res.body ?? [];
      this.paginationData = this.filtersvc.getPaginationHeader(res);
      this.csvSvc.getCategorizedProperties().subscribe((res) => {
        this.categorizedProperties = res;
        this.categorizedProperties.equalProperties.forEach((property) => {
          let equalPropertiesDataSource: EqualPropertiesDataSource = {
            key: property,
            fetcher: (key, searchString) =>
              this.csvSvc.getSearchValues(key, searchString),
            dataStore: [],
          };
          this.equalPropertiesDataSources.push(equalPropertiesDataSource);
        });

        this.filterRequest = this.filtersvc.populateFilterRequest(
          this.categorizedProperties,
          this.equalPropertiesDataSources
        );
      });
    });
  }
}
