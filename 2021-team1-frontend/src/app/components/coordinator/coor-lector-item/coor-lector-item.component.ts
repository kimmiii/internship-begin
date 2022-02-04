import {Component, Input, OnInit} from '@angular/core';
import {User} from "../../../models/user.model";

@Component({
  selector: 'app-coor-lector-item',
  templateUrl: './coor-lector-item.component.html',
  styleUrls: ['./coor-lector-item.component.css']
})
export class CoorLectorItemComponent implements OnInit {
  @Input() lector: User;

  constructor() { }

  ngOnInit() {
  }
}
