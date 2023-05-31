import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  title = 'Bike';
  apiURL = 'https://localhost:7250/'
  isCreate: boolean = false
  isEdit: boolean = false

  newBikeManufacture: string = ''
  newBikeModel: string = ''
  newBikeFrame: number = 12
  newBikePrice: number = 0.00

  editId: number = 0
  editBikeManufacture: string = ''
  editBikeModel: string = ''
  editBikeFrame: number = 12
  editBikePrice: number = 0.00

  bikeArray: {id: number, manufacture: string, model: string, frameSize: number, price: number}[] = []

  constructor(
    private httpClient: HttpClient
  ){

    this.httpClient.get(`${this.apiURL}Bike`)
    .subscribe((resp: any) => {
      console.log(resp)
      this.bikeArray = resp
    })
  }

  createNewBike(){
    if (this.newBikeManufacture == '') {
      alert('Please insert Manufacture!');
      return;
    }

    if (this.newBikeModel == '') {
      alert('Please insert Model!');
      return;
    }

    if (this.newBikeFrame <12 || this.newBikeFrame >62) {
      alert('Frame Size could be from 12 to 62!');
      return;
    }
    
    this.httpClient.post(`${this.apiURL}Bike`,
      {
        Manufacture: this.newBikeManufacture,
        Model: this.newBikeModel,
        FrameSize: this.newBikeFrame,
        Price: this.newBikePrice

      })
    .subscribe(() => {

      this.httpClient.get(`${this.apiURL}Bike`)
      .subscribe((resp: any) => {
        this.bikeArray = resp
  
      })
  
    })

    this.isCreate=false
    this.newBikeManufacture = ''
    this.newBikeModel = ''
    this.newBikeFrame = 12
    this.newBikePrice = 0.00

  }

  
  editBike(){
    this.httpClient.put(`${this.apiURL}Bike`,
    {
      id: this.editId,
      Manufacture: this.editBikeManufacture,
      Model: this.editBikeModel,
      FrameSize: this.editBikeFrame,
      Price: this.editBikePrice
    })
    .subscribe(() => {

      this.httpClient.get(`${this.apiURL}Bike`)
      .subscribe((resp: any) => {
        this.bikeArray = resp

      })

    })

    this.isEdit=false
    this.editId = 0
    this.editBikeManufacture = ''
    this.editBikeModel = ''
    this.editBikeFrame = 12
    this.editBikePrice = 0.00

  }

  deleteBike(id: number){
    this.httpClient.delete(`${this.apiURL}Bike/${id}`)
    .subscribe(() => {
      this.httpClient.get(`${this.apiURL}Bike`)
      .subscribe((resp: any) => {
        this.bikeArray = resp

      })

    })
  }

  edit(input: {id: number, manufacture: string, model: string, frameSize: number, price: number}){

    this.editId = input.id
    this.editBikeManufacture = input.manufacture
    this.editBikeModel = input.model
    this.editBikeFrame = input.frameSize
    this.editBikePrice = input.price

    this.isEdit = true
  }

  back(){
    if (this.isCreate) this.isCreate = false;
    if (this.isEdit) this.isEdit = false;
  }

}