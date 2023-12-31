import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-list-renderer',
  templateUrl: './list-renderer.component.html',
  styleUrls: ['./list-renderer.component.css'],
})
export class ListRendererComponent {
  @Input() data: any[] = [];
  columnHeaders: string[] = [];

  ngOnChanges(changes: SimpleChanges) {
    this.data = changes['data'].currentValue;
    this.columnHeaders = Object.keys(this.data[0]);
  }
}
