/*!
 * Star Rating Russian Translations
 *
 * This file must be loaded after 'star-rating.js'. Patterns in braces '{}', or
 * any HTML markup tags in the messages must not be converted or translated.
 *
 * NOTE: this file must be saved in UTF-8 encoding.
 *
 * @see http://github.com/kartik-v/bootstrap-star-rating
 * @author Kartik Visweswaran <kartikv2@gmail.com>
 * @author Ivan Zhuravlev.
 */
(function ($) {
    "use strict";
    $.fn.ratingLocales['ru'] = {
        defaultCaption: '{rating} звeзды',
        starCaptions: {
            0.5: '0.5 звезды',
            1: '1 звезда',
            1.5: '1.5 звезды',
            2: '2 звезды',
            2.5: '2.5 звезды',
            3: '3 звезды',
            3.5: '3.5 звезды',
            4: '4 звезды',
            4.5: '4.5 звезды',
            5: '5 звёзд'
        },
        clearButtonTitle: 'Очистить',
        clearCaption: 'Без рейтинга'
    };
})(window.jQuery);
