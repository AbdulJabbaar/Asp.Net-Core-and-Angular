import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core'

@Injectable()

export class PhotoService {
    constructor(private http: HttpClient) { }


    upload(vehicleId, photo) {
        let formData = new FormData()
        formData.append('file', photo)
        return this.http.post<any>(`http://localhost:49907/api/photos/${vehicleId}`, formData)
    }

    getPhotos(vehicleId){
        return this.http.get<any>(`http://localhost:49907/api/photos?vehicleId=${vehicleId}`)
    }
}