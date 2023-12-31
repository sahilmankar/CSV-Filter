import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { FiltersModule } from './filters/filters.module';
import { HttpClientModule } from '@angular/common/http';
import { ListRendererComponent } from './list-renderer/list-renderer.component';
import { CsvMainComponent } from './csv-main/csv-main.component';
import { FileUploadComponent } from './file-upload/file-upload.component';
import { Route, RouterModule, Routes } from '@angular/router';
import { NavbarComponent } from './navbar/navbar.component';

const routes: Route[] = [
  { path: '', pathMatch: 'full', redirectTo: 'file' },
  { path: 'file', component: FileUploadComponent },
  { path: 'workspace/:file', component: CsvMainComponent },
];
@NgModule({
  declarations: [
    AppComponent,
    ListRendererComponent,
    CsvMainComponent,
    FileUploadComponent,
    NavbarComponent,
  ],
  imports: [
    BrowserModule,
    FiltersModule,
    HttpClientModule,
    RouterModule.forRoot(routes),
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
