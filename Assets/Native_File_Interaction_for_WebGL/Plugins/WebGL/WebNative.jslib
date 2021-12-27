    var WebNative = {
       Alert: function(msgptr) {
         window.alert(Pointer_stringify(msgptr));
       },
       GetFile: function(filenameptr) {
         var filename = Pointer_stringify(filenameptr);
         var filedata = window.filedata[filename];
         var ptr = (window.fileptr = window.fileptr ? window.fileptr : {})[filename] = _malloc(filedata.byteLength);
         var dataHeap = new Uint8Array(HEAPU8.buffer, ptr, filedata.byteLength);
         dataHeap.set(new Uint8Array(filedata));
         return ptr;
       },
     
       UnloadFile: function(filenameptr) {
         var filename = Pointer_stringify(filenameptr);
         _free(window.fileptr[filename]);
         delete window.fileptr[filename];
         delete window.filedata[filename];

		 gameInstance.browserPopup = null;
       },
	   
	   CloseSelectFilePopup: function() {	
		 gameInstance.browserPopup = document.getElementById("showInfo");
		 gameInstance.browserPopup.style.display = "none";
	   },

       OpenSelectFilePopup: function(filenameptr) {
		var filename = Pointer_stringify(filenameptr);
		FilterOfTypes(filename);
		gameInstance.browserPopup = document.getElementById("showInfo");
		gameInstance.browserPopup.style.display = "block";
	   },

       SaveTextureToWebGLTexture: function (str, filename, texture) {
        
            //en:If you need to do processing in webgl, you may only need to use the texture object directly
            //cn:如果你需要在webgl中做处理，你可能只需要直接使用texture对象
            //en:Reserved:
            //cn:预留
            //GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);

            //The following is the operation to save the texture locally.

            var msg = Pointer_stringify(str);
            var fname = Pointer_stringify(filename);
            var contentType = 'image/jpeg';

            function fixBinary(bin) {
                var length = bin.length;
                var buf = new ArrayBuffer(length);
                var arr = new Uint8Array(buf);
                for (var i = 0; i < length; i++) {
                    arr[i] = bin.charCodeAt(i);
                }
                return buf;
            }
            //en:atob decodes a string encoded using base64
            //cn:atob解码使用base64编码的字符串
            var binary = fixBinary(atob(msg));

            var data = new Blob([binary], { type: contentType });
            var link = document.createElement('a');
            link.download = filename;
            link.innerHTML = 'DownloadFile';
            link.setAttribute('id', 'ImageDownloaderLink');
            link.href = window.URL.createObjectURL(data);
            link.onclick = function () {
                var child = document.getElementById('ImageDownloaderLink');
                child.parentNode.removeChild(child);
            };
            link.style.display = 'none';
            document.body.appendChild(link);
            link.click();
            window.URL.revokeObjectURL(link.href);

       },
       FileLength: function(filenameptr) {
         var filename = Pointer_stringify(filenameptr);
         return window.filedata[filename].byteLength;
       },
       SaveText: function(filename, text) {
              var element = document.createElement('a');
              element.setAttribute('href', 'data:text/plain;charset=x-user-defined,' + encodeURIComponent(Pointer_stringify(text)));
              element.setAttribute('download', Pointer_stringify(filename));
              element.style.display = 'none';
              document.body.appendChild(element);
              element.click();
              document.body.removeChild(element);
       },
    };
     
    mergeInto(LibraryManager.library, WebNative);