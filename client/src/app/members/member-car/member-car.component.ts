import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';

@Component({
  selector: 'app-member-car',
  templateUrl: './member-car.component.html',
  styleUrls: ['./member-car.component.css'],
})
export class MemberCarComponent implements OnInit {

  @Input() member: Member;

  constructor() { }

  ngOnInit(): void {
  }

} 
