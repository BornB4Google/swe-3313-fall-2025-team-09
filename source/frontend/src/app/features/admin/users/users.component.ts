import { Component } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent {

  /*temp holder for backend*/
  users = [
    { id: 1, email: "name1@example.com", name: "Name Name1", role: "User" },
    { id: 2, email: "name2@example.com", name: "Name Name2", role: "Admin" },
    { id: 3, email: "name3@example.com", name: "Name Name3", role: "User" },
    { id: 4, email: "name4@example.com", name: "Name Name4", role: "User" },
    { id: 5, email: "name5@example.com", name: "Name Name5", role: "User" }
  ];

  promoteToAdmin(user: any) {
    user.role = "Admin";
  }
}
