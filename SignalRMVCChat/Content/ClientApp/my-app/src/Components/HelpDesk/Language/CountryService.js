import {Countries} from "./Countries";


export class CountryService{
    
    GetAll(){
        return new Promise(resolve => {

            resolve(Countries);
        });
    }
}