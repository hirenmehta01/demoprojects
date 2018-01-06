import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductListComponent } from './products/products-list.component';
import { AngularHelp } from './angularhelp/angularhelp.component'
const appRoutes: Routes = [
    {
        path: 'products',
        component: ProductListComponent
    },
    {
        path: 'usefullinks',
        component: AngularHelp
    },
    {
        path: '**',  // otherwise route.
        component: AngularHelp
    }
];
export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);