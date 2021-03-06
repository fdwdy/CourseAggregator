import { Component, OnInit, SkipSelf } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  loginForm: FormGroup;
id: string;
constructor(
  @SkipSelf()  fb: FormBuilder,
  private loader: NgxUiLoaderService,
  route: ActivatedRoute) {
    route.params.subscribe(params => this.id = params['id']),
  this.loginForm = fb.group({
    oldPassword: [''],
    newPassword: [''],
    newPasswordConfirm: ['']
  });
}

  ngOnInit() {
  }
  onSubmit() {
    if (this.loginForm.valid) {
      const data = this.loginForm.value;

      this.loader.start();
      this.loader.stop();
    }
  }

}
