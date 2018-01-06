import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import { Link } from '../usefullLinksService/link';
import { UsefullLinkService } from '../usefullLinksService/usefulllink.service';

@Component({
    selector: 'abgular-help',
    templateUrl: './angularhelp.component.html',
    providers: [UsefullLinkService]
})

export class AngularHelp implements OnInit {
    private _usefullLinkService: UsefullLinkService;
    private _linkList: Link[]
    get LinkList() { return this._linkList; }
    constructor(private linkService: UsefullLinkService) {
        this._usefullLinkService = linkService;
    }
    public ngOnInit(): void {
        this._linkList = this._usefullLinkService.GetLinks();
    }
}