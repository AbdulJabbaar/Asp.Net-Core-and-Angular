import { KeyValuePair } from './../models/vehicle'
import { Component, OnInit } from '@angular/core'
import { Vehicle } from '../models/vehicle'
import { VehicleService } from '../services/vehicle.service'
import { forkJoin } from 'rxjs'

@Component({
  selector: 'app-vehicle',
  templateUrl: './vehicle.component.html',
  styleUrls: ['./vehicle.component.css']
})
export class VehicleComponent implements OnInit {

  vehicles: Vehicle[]
  makes: KeyValuePair[]
  filterResource: any = {
    pageSize: 3
  }
  columns =[
    {title: 'Id'},    
    {title: 'Make', key:'make', isSortable: true},
    {title: 'Model', key:'model', isSortable: true},
    {title: 'Contact Name', key:'contactName', isSortable: true}
  ]
  totalItems: number

  constructor(private vehicleService: VehicleService) { }

  ngOnInit() {
    forkJoin([this.vehicleService.getVehicles(this.filterResource), this.vehicleService.getMakes()]).subscribe(res => {
      this.vehicles = res[0].items
      this.totalItems= res[0].totalItems
      this.makes = res[1]
    })
  }

  onFilterChange() {
    this.vehicleService.getVehicles(this.filterResource).subscribe(res => {
      this.vehicles = res.items
      this.totalItems= res.totalItems
    })
  }

  sortBy(columnName) {
    if (this.filterResource.sortBy === columnName) {
      this.filterResource.isSortAscending = !this.filterResource.isSortAscending
    }
    else {
      this.filterResource.sortBy = columnName
      this.filterResource.isSortAscending = true
    }
    this.onFilterChange()
  }

  resetFilter() {
    this.filterResource = {}    
    this.filterResource.page = 1
    this.filterResource.pageSize = 3
    this.onFilterChange()
  }

  onPageChange(page){
    this.filterResource.page = page
    this.onFilterChange() 
  }
}
