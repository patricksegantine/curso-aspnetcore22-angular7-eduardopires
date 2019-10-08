import { Injectable, Inject } from '@angular/core';
import { Title } from '@angular/platform-browser';import { DOCUMENT } from "@angular/common";
import { SeoModel } from './seo.model';
import { StringUtils } from 'src/app/utils/string.utils';

@Injectable({
  providedIn: 'root'
})
export class SeoService {
  private titleService: Title;
  private headElement: HTMLElement;
  private metaDescription: HTMLElement;
  private metaKeywords: HTMLElement;
  private robots: HTMLElement;
  private DOM: any;

  public constructor(@Inject(DOCUMENT) private document, titleService: Title) { 
    this.titleService = titleService;
    this.DOM = document;
    this.headElement = this.DOM.head; //this.dom.query('head');
  }

  public setSeoData(seoModel: SeoModel) {
    this.setTitle(seoModel.title);
    this.setMetaRobots(seoModel.robots);
    this.setMetaDescription(seoModel.description);
    this.setMetaKeywords(seoModel.keywords);
  }
 
  //////

  private setTitle(newTitle: string) {
    if(StringUtils.isNullOrEmpty(newTitle)) {
      newTitle = "Defina um Título";
    }
    this.titleService.setTitle(newTitle + " - Eventos.IO");
  }

  private setMetaDescription(description: string){
    if(StringUtils.isNullOrEmpty(description)) {
      description = "Aqui você encontra um evento técnico próximo de você";
    }
    this.metaDescription = this.getOrCreateMetaElement('description');
    this.metaDescription.setAttribute('content', description);
  }

  private setMetaKeywords(keywords: string ) {
    if(StringUtils.isNullOrEmpty(keywords)) { 
      keywords = "eventos,workshops,encontros,congressos,comunidades,tecnologia"
    }
    this.metaKeywords = this.getOrCreateMetaElement('keywords');
    this.metaKeywords.setAttribute('content', keywords);
  }

  private setMetaRobots(robots: string) {
    if(StringUtils.isNullOrEmpty(robots)) {
      robots = "all";
    }
    this.robots = this.getOrCreateMetaElement('robots');
    this.robots.setAttribute('content', robots);
  }

  private getOrCreateMetaElement(name:string): HTMLElement {
    let el: HTMLElement;
    el = this.document.querySelector('meta[name=' + name + ']');
    if(el === null) {
      el = this.DOM.createElement('meta');
      el.setAttribute('name', name);
      this.headElement.appendChild(el);
    }
    return el;
  }
}


