import {Component, inject, Signal} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {CommonModule} from '@angular/common';
import {AdminUserService} from '../../../services/admin-user/admin-user.service';
import {toSignal} from '@angular/core/rxjs-interop';
import {User} from '../../../models/user.models';

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

  adminUserService = inject(AdminUserService);


  users = toSignal(this.adminUserService.getUsers(),{ initialValue: [] as User[] });

  promoteToAdmin(user: any) {
    user.role = "Admin";
  }
}
