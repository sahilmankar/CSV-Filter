import {
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { PaginationHeader } from '../paginationHeader';

@Component({
  selector: 'pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css'],
})
export class PaginationComponent {
  @Input() paginationData: PaginationHeader | null = null;
  @Output() pageChangeEvent = new EventEmitter<number>();

  // onClickPrevious() {
  //   if (this.paginationData) {
  //     let pageNumber = this.paginationData.CurrentPage - 1;
  //     this.pageChangeEvent.emit(pageNumber);
  //   }
  // }

  // onClickNext() {
  //   if (this.paginationData) {
  //     let pageNumber = this.paginationData.CurrentPage + 1;
  //     this.pageChangeEvent.emit(pageNumber);
  //   }
  // }

  goToPage(page: number): void {
    if(this.paginationData)
    if (page >= 1 && page <= this.paginationData.TotalPages && page !== this.paginationData.CurrentPage) {
      this.pageChangeEvent.emit(page);
    }
  }

  getPageNumbers(): number[] {
    if(!this.paginationData){
      return [];
    }
    const startPage = Math.max(1, this.paginationData.CurrentPage - 2);
    const endPage = Math.min(this.paginationData.TotalPages, this.paginationData.CurrentPage + 2);

    return Array.from({ length: endPage - startPage + 1 }, (_, index) => startPage + index);
  }

}
