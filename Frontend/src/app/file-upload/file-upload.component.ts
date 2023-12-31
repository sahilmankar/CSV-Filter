import {
  Component,
  ElementRef,
  EventEmitter,
  Output,
  ViewChild,
} from '@angular/core';
import { CsvService } from '../csv.service';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css'],
})
export class FileUploadComponent {
  selectedFile: File | undefined;
  uploadProgress: number | null = null;

  constructor(private csvService: CsvService, private router: Router) {}

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  onUpload() {
    if (this.selectedFile) {
      const formData = new FormData();

      formData.append('file', this.selectedFile, this.selectedFile.name);

      this.csvService.uploadFile(this.selectedFile.name, formData).subscribe({
        next: (event) => {
          if (event.type === HttpEventType.UploadProgress) {
            this.uploadProgress = Math.round(
              (100 * event.loaded) / event.total
            );
          } else if (event instanceof HttpResponse) {
            let filename: string = event.body.fileName;
            this.router.navigate(['workspace', filename]);
            this.uploadProgress = null;
          }
        },
      });
    }
  }
}
