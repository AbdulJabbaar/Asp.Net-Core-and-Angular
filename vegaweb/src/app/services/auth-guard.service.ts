import { Injectable } from '@angular/core'
import { AuthService } from './auth.service'
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router'
import { Observable } from 'rxjs'
@Injectable()

export class AuthGuard implements CanActivate {
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (this.auth.isAuthenticated()) {
            return true
        }
        return false
    }
    constructor(private auth: AuthService) { }
}