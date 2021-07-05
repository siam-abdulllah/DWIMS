import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent implements OnInit {

  validationErrors: any;
  constructor(private http: HttpClient) { }

  baseUrl = environment.apiUrl;
  ngOnInit() {
  }

  get404Error(){
    this.http.get(this.baseUrl + 'product/43').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(console.log(error));
    });
  }

  get500Error(){
    this.http.get(this.baseUrl + 'buggy/servererror').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(console.log(error));
    });
  }
  get400Error(){
    this.http.get(this.baseUrl + 'buggy/badrequest').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(console.log(error));
    });
  }
  get400ValidationError(){
    this.http.get(this.baseUrl + 'products/fortythree').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(console.log(error));
      this.validationErrors = error.errors ;
    });
  }
}
