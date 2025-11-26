import {Component, inject} from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from '../../services/auth/auth.service';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  imports: [
    RouterLink,
    FormsModule
  ],
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
 authService = inject(AuthService);
  router = inject(Router);
 username:string = "";
 password:string = "";

  login(){
    // console.log('username:' +this.username, ' password:' + this.password);
    this.authService.login(this.username, this.password).subscribe({
      // This block runs if the request is SUCCESSFUL (status 2xx)
      next: (response) => {
        console.log('Login Successful!');
        console.log('Server Response:', response);
        this.router.navigate(['/inventory']);

      },
      error: (err)=>{
        console.log('Login failed!');
        console.log('Server Response:', err);
      }

      },

      );
  }

}
