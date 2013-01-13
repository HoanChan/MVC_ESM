/// <reference path="http://code.jquery.com/jquery-1.4.1-vsdoc.js" />
/*
* Print Element Plugin 1.2
*
* Copyright (c) 2010 Erik Zaadi
*
* Inspired by PrintArea (http://plugins.jquery.com/project/PrintArea) and
* http://stackoverflow.com/questions/472951/how-do-i-print-an-iframe-from-javascript-in-safari-chrome
*
*  Home Page : http://projects.erikzaadi/jQueryPlugins/jQuery.printElement 
*  Issues (bug reporting) : http://github.com/erikzaadi/jQueryPlugins/issues/labels/printElement
*  jQuery plugin page : http://plugins.jquery.com/project/printElement 
*  
*  Thanks to David B (http://github.com/ungenio) and icgJohn (http://www.blogger.com/profile/11881116857076484100)
*  For their great contributions!
* 
* Dual licensed under the MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
*   
*   Note, Iframe Printing is not supported in Opera and Chrome 3.0, a popup window will be shown instead
*/
; (function (window, undefined) {
    var document = window["document"];
    var $ = window["jQuery"];
    $.fn["printElement"] = function (options) {
        var mainOptions = $.extend({}, $.fn["printElement"]["defaults"], options);
        //iframe mode is not supported for opera and chrome 3.0 (it prints the entire page).
        //http://www.google.com/support/forum/p/Webmasters/thread?tid=2cb0f08dce8821c3&hl=en
        if (mainOptions["printMode"] == 'iframe') {
            if ($.browser.opera || (/chrome/.test(navigator.userAgent.toLowerCase())))
                mainOptions["printMode"] = 'popup';
        }
        //Remove previously printed iframe if exists
        $("[id^='printElement_']").remove();

        return this.each(function () {
            //Support Metadata Plug-in if available
            var opts = $.meta ? $.extend({}, mainOptions, $(this).data()) : mainOptions;
            _printElement($(this), opts);
        });
    };
    $.fn["printElement"]["defaults"] = {
        "printMode": 'iframe', //Usage : iframe / popup
        "pageTitle": '', //Print Page Title
        "overrideElementCSS": null,
        /* Can be one of the following 3 options:
        * 1 : boolean (pass true for stripping all css linked)
        * 2 : array of $.fn.printElement.cssElement (s)
        * 3 : array of strings with paths to alternate css files (optimized for print)
        */
        "printBodyOptions": {
            "styleToAdd": 'padding:10px;margin:10px;', //style attributes to add to the body of print document
            "classNameToAdd": '' //css class to add to the body of print document
        },
        "leaveOpen": false, // in case of popup, leave the print page open or not
        "iframeElementOptions": {
            "styleToAdd": 'border:none;position:absolute;width:0px;height:0px;bottom:0px;left:0px;', //style attributes to add to the iframe element
            "classNameToAdd": '' //css class to add to the iframe element
        }
    };
    $.fn["printElement"]["cssElement"] = {
        "href": '',
        "media": ''
    };
    function _printElement(element, opts) {
        //Create markup to be printed
        var html = _getMarkup(element, opts);

        var popupOrIframe = null;
        var documentToWriteTo = null;
        if (opts["printMode"].toLowerCase() == 'popup') {
            popupOrIframe = window.open('about:blank', 'printElementWindow', 'width=650,height=440,scrollbars=yes');
            documentToWriteTo = popupOrIframe.document;
        }
        else {
            //The random ID is to overcome a safari bug http://www.cjboco.com.sharedcopy.com/post.cfm/442dc92cd1c0ca10a5c35210b8166882.html
            var printElementID = "printElement_" + (Math.round(Math.random() * 99999)).toString();
            //Native creation of the element is faster..
            var iframe = document.createElement('IFRAME');
            $(iframe).attr({
                style: opts["iframeElementOptions"]["styleToAdd"],
                id: printElementID,
                className: opts["iframeElementOptions"]["classNameToAdd"],
                frameBorder: 0,
                scrolling: 'no',
                src: 'about:blank'
            });
            document.body.appendChild(iframe);
            documentToWriteTo = (iframe.contentWindow || iframe.contentDocument);
            if (documentToWriteTo.document)
                documentToWriteTo = documentToWriteTo.document;
            iframe = document.frames ? document.frames[printElementID] : document.getElementById(printElementID);
            popupOrIframe = iframe.contentWindow || iframe;
        }
        focus();
        documentToWriteTo.open();
        documentToWriteTo.write(html);
        documentToWriteTo.close();
        _callPrint(popupOrIframe);
    };

    function _callPrint(element) {
        if (element && element["printPage"])
            element["printPage"]();
        else
            setTimeout(function () {
                _callPrint(element);
            }, 50);
    }

    function _getElementHTMLIncludingFormElements(element) {
        var $element = $(element);
        //Radiobuttons and checkboxes
        $(":checked", $element).each(function () {
            this.setAttribute('checked', 'checked');
        });
        //simple text inputs
        $("input[type='text']", $element).each(function () {
            this.setAttribute('value', $(this).val());
        });
        $("select", $element).each(function () {
            var $select = $(this);
            $("option", $select).each(function () {
                if ($select.val() == $(this).val())
                    this.setAttribute('selected', 'selected');
            });
        });
        $("textarea", $element).each(function () {
            //Thanks http://blog.ekini.net/2009/02/24/jquery-getting-the-latest-textvalue-inside-a-textarea/
            var value = $(this).attr('value');
            //fix for issue 7 (http://plugins.jquery.com/node/13503 and http://github.com/erikzaadi/jQueryPlugins/issues#issue/7)
            if ($.browser.mozilla && this.firstChild)
                this.firstChild.textContent = value;
            else
                this.innerHTML = value;
        });
        //http://dbj.org/dbj/?p=91
        var elementHtml = $('<div></div>').append($element.clone()).html();
        return elementHtml;
    }

    function _getBaseHref() {
        var port = (window.location.port) ? ':' + window.location.port : '';
        return window.location.protocol + '//' + window.location.hostname + port + window.location.pathname;
    }

    function _getMarkup(element, opts) {
        var $element = $(element);
        var elementHtml = _getElementHTMLIncludingFormElements(element);

        var html = new Array();
        html.push('<html><head><title>' + opts["pageTitle"] + '</title>');
        if (opts["overrideElementCSS"]) {
            if (opts["overrideElementCSS"].length > 0) {
                for (var x = 0; x < opts["overrideElementCSS"].length; x++) {
                    var current = opts["overrideElementCSS"][x];
                    if (typeof (current) == 'string')
                        html.push('<link type="text/css" rel="stylesheet" href="' + current + '" >');
                    else
                        html.push('<link type="text/css" rel="stylesheet" href="' + current["href"] + '" media="' + current["media"] + '" >');
                }
            }
        }
        else {
            $("link", document).filter(function () {
                return $(this).attr("rel").toLowerCase() == "stylesheet";
            }).each(function () {
                html.push('<link type="text/css" rel="stylesheet" href="' + $(this).attr("href") + '" media="' + $(this).attr('media') + '" >');
            });
        }
        //Ensure that relative links work
        html.push('<base href="' + _getBaseHref() + '" />');
        html.push('</head><body style="' + opts["printBodyOptions"]["styleToAdd"] + '" class="' + opts["printBodyOptions"]["classNameToAdd"] + '">');
        html.push('<div class="' + $element.attr('class') + '">' + elementHtml + '</div>');
        html.push('<script type="text/javascript">function printPage(){focus();print();' + ((!$.browser.opera && !opts["leaveOpen"] && opts["printMode"].toLowerCase() == 'popup') ? 'close();' : '') + '}</script>');
        html.push('</body></html>');

        return html.join('');
    };
})(window);

//=====================================================================================================================//

/*!
 * jQuery Print Previw Plugin v1.0.1
 *
 * Copyright 2011, Tim Connell
 * Licensed under the GPL Version 2 license
 * http://www.gnu.org/licenses/gpl-2.0.html
 *
 * Date: Wed Jan 25 00:00:00 2012 -000
 */

(function ($) {

	// Initialization
	$.fn.printPreview = function () {
		this.each(function () {
			$(this).bind('click', function (e) {
				e.preventDefault();
				if (!$('#print-modal').length) {
					$.printPreview.loadPrintPreview();
				}
			});
		});
		return this;
	};

	// Private functions
	var mask, size, print_modal, print_controls;
	$.printPreview = {
		loadPrintPreview: function () {
			// Declare DOM objects
			print_modal = $('<div id="print-modal"></div>');
			print_controls = $('<div id="print-modal-controls">' +
                                    '<a href="#" class="print" title="Print page">Print page</a>' +
                                    '<a href="#" class="close" title="Close print preview">Close</a>').hide();
			var print_frame = $('<iframe id="print-modal-content" scrolling="no" border="0" frameborder="0" name="print-frame" />');

			// Raise print preview window from the dead, zooooooombies
			print_modal
                .hide()
                .append(print_controls)
                .append(print_frame)
                .appendTo('body');

			// The frame lives
			for (var i = 0; i < window.frames.length; i++) {
				if (window.frames[i].name == "print-frame") {
					var print_frame_ref = window.frames[i].document;
					break;
				}
			}
			print_frame_ref.open();
			print_frame_ref.write('<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">' +
                '<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">' +
                '<head><title>' + document.title + '</title></head>' +
                '<body><link href="/Content/Printer.css" rel="stylesheet" type="text/css" /></body>' +
                '</html>');
			print_frame_ref.close();

			// Grab contents and apply stylesheet
			var $iframe_head = $('head link[media*=print], head link[media=all]').clone();
			//var $iframe_body = $('body > *:not(#print-modal):not(script)').clone();			
			var $iframe_body = "<table>";
			$iframe_body += "<thead>" + $("#table>thead").html() + "</thead><tbody>";
			var oTable = $('#table').dataTable();

			oTable.$('tr').each(function (index, itemData) {
				$iframe_body += "<tr>" + $(this).html() + "</tr>"
			});

			$iframe_body += "</tbody></table>";

			$iframe_head.each(function () {
				$(this).attr('media', 'all');
			});
			if (!$.browser.msie && !($.browser.version < 7)) {
				$('head', print_frame_ref).append($iframe_head);
				$('body', print_frame_ref).append($iframe_body);
			}
			else {
				$('body > *:not(#print-modal):not(script)').clone().each(function () {
					$('body', print_frame_ref).append(this.outerHTML);
				});
				$('head link[media*=print], head link[media=all]').each(function () {
					$('head', print_frame_ref).append($(this).clone().attr('media', 'all')[0].outerHTML);
				});
			}

			// Disable all links
			$('a', print_frame_ref).bind('click.printPreview', function (e) {
				e.preventDefault();
			});

			// Introduce print styles
			$('head').append('<style type="text/css">' +
                '@media print {' +
                    '/* -- Print Preview --*/' +
                    '#print-modal-mask,' +
                    '#print-modal {' +
                        'display: none !important;' +
                    '}' +
                '}' +
                '</style>'
            );

			// Load mask
			$.printPreview.loadMask();

			// Disable scrolling
			$('body').css({ overflowY: 'hidden', height: '100%' });
			$('img', print_frame_ref).load(function () {
				print_frame.height($('body', print_frame.contents())[0].scrollHeight);
			});

			// Position modal            
			starting_position = $(window).height() + $(window).scrollTop();
			var css = {
				top: starting_position,
				height: '100%',
				overflowY: 'auto',
				zIndex: 10000,
				display: 'block'
			}
			print_modal
                .css(css)
                .animate({ top: $(window).scrollTop() }, 400, 'linear', function () {
                	print_controls.fadeIn('slow').focus();
                });
			print_frame.height($('body', print_frame.contents())[0].scrollHeight);

			// Bind closure
			$('a', print_controls).bind('click', function (e) {
				e.preventDefault();
				if ($(this).hasClass('print')) {
				    //window.print();
				    $('body', print_frame_ref).printElement({ overrideElementCSS: ['/Content/Printer.css'] })
				}
				else {
				    $.printPreview.distroyPrintPreview();
				}
			});
		},

		distroyPrintPreview: function () {
			print_controls.fadeOut(100);
			print_modal.animate({ top: $(window).scrollTop() - $(window).height(), opacity: 1 }, 400, 'linear', function () {
				print_modal.remove();
				$('body').css({ overflowY: 'auto', height: 'auto' });
			});
			mask.fadeOut('slow', function () {
				mask.remove();
			});

			$(document).unbind("keydown.printPreview.mask");
			mask.unbind("click.printPreview.mask");
			$(window).unbind("resize.printPreview.mask");
		},

		/* -- Mask Functions --*/
		loadMask: function () {
			size = $.printPreview.sizeUpMask();
			mask = $('<div id="print-modal-mask" />').appendTo($('body'));
			mask.css({
				position: 'absolute',
				top: 0,
				left: 0,
				width: size[0],
				height: size[1],
				display: 'none',
				opacity: 0,
				zIndex: 9999,
				backgroundColor: '#000'
			});

			mask.css({ display: 'block' }).fadeTo('400', 0.75);

			$(window).bind("resize..printPreview.mask", function () {
				$.printPreview.updateMaskSize();
			});

			mask.bind("click.printPreview.mask", function (e) {
				$.printPreview.distroyPrintPreview();
			});

			$(document).bind("keydown.printPreview.mask", function (e) {
				if (e.keyCode == 27) { $.printPreview.distroyPrintPreview(); }
			});
		},

		sizeUpMask: function () {
			if ($.browser.msie) {
				// if there are no scrollbars then use window.height
				var d = $(document).height(), w = $(window).height();
				return [
            		window.innerWidth || 						// ie7+
            		document.documentElement.clientWidth || 	// ie6  
            		document.body.clientWidth, 					// ie6 quirks mode
            		d - w < 20 ? w : d
				];
			} else { return [$(document).width(), $(document).height()]; }
		},

		updateMaskSize: function () {
			var size = $.printPreview.sizeUpMask();
			mask.css({ width: size[0], height: size[1] });
		}
	}
})(jQuery);


