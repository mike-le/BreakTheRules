import { Injectable } from "@angular/core";

@Injectable()
export class UserPrincipal{
    isAppAdmin: boolean;
    isBlacklisted: boolean;
    displayName: string;
    directReports: string[];
}

