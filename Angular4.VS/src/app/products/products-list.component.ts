import{Component} from '@angular/core'
@Component({
selector : 'pm-products',
templateUrl : './products-list.component.html'
})
export class ProductListComponent
{
    pageTitle : string = 'Products';
    imageWidth : number = 50;
    imageMargin : number = 2;
    products : any[] = [
        {
            "productId": 1,
            "productName": "Washing Machine",
            "productCode": "WS",
            "releaseDate": "March 19, 2016",
            "description": "Fully automated machine",
            "price": 200,
            "starRating": 3.2,
            "imageUrl": "https://github.com/hirenmehta01/demoprojects/raw/dev/images/1.jpg"
        },
        {
            "productId": 2,
            "productName": "Mobile",
            "productCode": "MB",
            "releaseDate": "March 18, 2016",
            "description": "smart phone",
            "price": 300,
            "starRating": 4.2,
            "imageUrl": "https://github.com/hirenmehta01/demoprojects/raw/dev/images/2.jpg"
        },
        {
            "productId": 5,
            "productName": "Harddisk",
            "productCode": "HD",
            "releaseDate": "May 21, 2016",
            "description": "Sata harddisk",
            "price": 8.9,
            "starRating": 4.8,
            "imageUrl": "https://github.com/hirenmehta01/demoprojects/raw/dev/images/3.jpg"
        },
        {
            "productId": 8,
            "productName": "Nugen",
            "productCode": "NG",
            "releaseDate": "May 15, 2016",
            "description": "Nugen deice for exercises",
            "price": 11.55,
            "starRating": 3.7,
            "imageUrl": "https://github.com/hirenmehta01/demoprojects/raw/dev/images/4.jpg"
        },
        {
            "productId": 10,
            "productName": "Printer",
            "productCode": "PR",
            "releaseDate": "October 15, 2015",
            "description": "color printer",
            "price": 35.95,
            "starRating": 4.6,
            "imageUrl": "https://github.com/hirenmehta01/demoprojects/raw/dev/images/5.jpg"
        }
    ];
}