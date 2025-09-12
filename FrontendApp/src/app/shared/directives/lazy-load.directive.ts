import { Directive, ElementRef, EventEmitter, OnInit, OnDestroy, Output, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Directive({
  selector: '[appLazyLoad]',
  standalone: true
})
export class LazyLoadDirective implements OnInit, OnDestroy {
  @Output() lazyLoad = new EventEmitter<void>();

  private observer?: IntersectionObserver;
  private isBrowser: boolean;

  constructor(
    private elementRef: ElementRef,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(this.platformId);
  }

  ngOnInit() {
    if (!this.isBrowser) {
      // En el servidor, emitir inmediatamente
      this.lazyLoad.emit();
      return;
    }

    if ('IntersectionObserver' in window) {
      this.observer = new IntersectionObserver(
        (entries) => {
          entries.forEach(entry => {
            if (entry.isIntersecting) {
              this.lazyLoad.emit();
              this.unobserve();
            }
          });
        },
        {
          rootMargin: '100px 0px', // Cargar 100px antes de que sea visible
          threshold: 0.1
        }
      );

      this.observer.observe(this.elementRef.nativeElement);
    } else {
      // Fallback para navegadores sin soporte
      this.lazyLoad.emit();
    }
  }

  ngOnDestroy() {
    this.unobserve();
  }

  private unobserve() {
    if (this.observer) {
      this.observer.disconnect();
      this.observer = undefined;
    }
  }
}
