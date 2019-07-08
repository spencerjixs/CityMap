import { Component, OnInit } from '@angular/core';
import { CityMapService } from 'src/app/city-map/city-map.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-city-map',
  templateUrl: './city-map.component.html',
  styles: []
})
export class CityMapComponent implements OnInit {

  constructor(private service: CityMapService,
    private toastr: ToastrService) { }

  showResult = false;
  showError = false;
  ngOnInit() {
    this.resetForm();
  }
  zipCodeChanged(newObj){
    this.showResult = false;
    this.showError = false;
  }
  resetForm(form?: NgForm) {
    if (form != null){
      form.resetForm();
    }
    this.service.formData = {
      city: '',
      temperature: null,
      timeZone: '',
      elevation: null,
      latitude : null,
      longitude : null,
      zipCode: ''
    }
  }

  onSubmit(form: NgForm) {
    if (form.value.zipCode != null){
      this.service.getCityMap(form.value.zipCode).subscribe(
      (data) =>
      {
        debugger;
        this.resetForm(form);
        this.service.formData = data;
        this.showResult = true;
        this.showError = false;
        this.toastr.success('Found result successfully', 'City Weather');
      }, 
      (error) =>
      {
        debugger;
        console.log("error", error)
        this.resetForm(form);
        this.showError = true;
        this.showResult = false;
        this.toastr.error('No result found', 'City Weather');        
      });
    }
  }

}