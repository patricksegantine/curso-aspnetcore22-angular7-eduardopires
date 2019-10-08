import { HttpHeaders } from '@angular/common/http';
import { throwError } from 'rxjs';

export abstract class ServiceBase {
    protected UrlServiceV1 : string = "https://localhost:44355/api/v1";
    
    protected ObterHeaderJson() {
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        }
    }

    protected extractData(response: any) {
        return response.data || {};
    }

    protected serviceError(error: Response | any) {
        let errorMessage: string;

        if(error instanceof Response) {
            errorMessage = `${error.status} - ${error.statusText || ''}`;
        } else {
            errorMessage = error.errorMessage ? error.message : error.toString();
        }
        return throwError(error);
    }

    public setLocalStorage(token: string, user: string) {
        localStorage.setItem('target.io-token', token);
        localStorage.setItem('target.io-user', user);
    }

    public getUserToken(): string {
        return localStorage.getItem('target.io-token');
    }

    public getUser(): string {
        return localStorage.getItem('target.io-user');
    }

    public removeUserToken() {
        localStorage.removeItem('target.io-token');
        localStorage.removeItem('target.io-user');
    }

    public isAuthenticated() : boolean {
        let token = this.getUserToken();

        return token !== null;
    }
}