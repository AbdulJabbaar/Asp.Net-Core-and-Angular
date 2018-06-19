import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'

@Injectable({
  providedIn: 'root'
})
export class VehicleService {

  constructor(private http: HttpClient) { }

  getMakes() {
    return this.http.get<any>('http://localhost:49907/api/makes')
  }

  getFeatures() {
    return this.http.get<any>('http://localhost:49907/api/features')
  }

  create(vehicle) {
    return this.http.post<any>('http://localhost:49907/api/vehicles', vehicle)
  }

  getVehicles(filter) {
    return this.http.get<any>('http://localhost:49907/api/vehicles/' + '?' + this.toQueryString(filter))
  }

  toQueryString(obj) {
    let parts = []
    for (let prop in obj) {
      let value = obj[prop]
      if (value != null && value != undefined)
        parts.push(encodeURIComponent(prop) + '=' + encodeURIComponent(value))
    }
    return parts.join('&')
  }

  getVehicle(id) {
    return this.http.get<any>('http://localhost:49907/api/vehicles/' + id)
  }

  update(vehicle) {
    return this.http.put<any>('http://localhost:49907/api/vehicles/' + vehicle.id, vehicle)
  }

  delete(id) {
    return this.http.delete<any>('http://localhost:49907/api/vehicles/' + id)
  }
}
