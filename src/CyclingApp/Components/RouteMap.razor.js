export function getBoundingRectangle(element) {
    return element.getBoundingClientRect();
}

export function createResizeObserver(element, instance, callbackName) {
    new ResizeObserver(() => instance.invokeMethodAsync(callbackName)).observe(element);
}