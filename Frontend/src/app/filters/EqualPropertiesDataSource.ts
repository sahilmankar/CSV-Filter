import { Observable } from 'rxjs';

export class EqualPropertiesDataSource {
  constructor(
    public key: string,
    public fetcher: (key:string, searchString: string) => Observable<string[]>,
    public dataStore: string[]
  ) {}
}
