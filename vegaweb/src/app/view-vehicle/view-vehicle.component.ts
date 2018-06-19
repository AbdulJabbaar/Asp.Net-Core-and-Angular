import { ProgressService } from './../services/progress.service';
import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { VehicleService } from '../services/vehicle.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PhotoService } from '../services/photo.service';

@Component({
  selector: 'app-view-vehicle',
  templateUrl: './view-vehicle.component.html',
  styleUrls: ['./view-vehicle.component.css']
})
export class ViewVehicleComponent implements OnInit {

  @ViewChild('fileInput') fileInput: ElementRef
  vehicleId: number
  vehicle: any
  photos: any[]

  constructor(
    private vehicleService: VehicleService,
    private route: ActivatedRoute,
    private router: Router,
    private photoService: PhotoService,
    private progressService: ProgressService
  ) {
    route.params.subscribe(p => {
      this.vehicleId = +p['id']
      if (isNaN(this.vehicleId) || this.vehicleId <= 0) {
        router.navigate(['/vehicles'])
        return
      }
    })
  }

  ngOnInit() {
    this.photoService.getPhotos(this.vehicleId).subscribe(
      photos => this.photos = photos
    )

    this.vehicleService.getVehicle(this.vehicleId).subscribe(res => {
      this.vehicle = res,
        err => {
          if (err.status == 404) {
            this.router.navigate(['/vehicles'])
            return
          }
        }
    })
  }

  delete() {
    if (confirm("Are you sure?")) {
      this.vehicleService.delete(this.vehicleId).subscribe(res => {
        this.router.navigate(['/vehicles'])
      })
    }
  }

  uploadPhoto() {
    let nativeELement = this.fileInput.nativeElement;
    this.progressService.uploadProgress.subscribe(prog=>console.log(prog))
    this.photoService.upload(this.vehicleId, nativeELement.files[0])
      .subscribe(photo => {
        this.photos.push(photo)
      })
  }
}
