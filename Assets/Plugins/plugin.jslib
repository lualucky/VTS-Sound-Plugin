var plugin = {
    GetTwitchSecrets: function()
    {
        var cn = channel_name;
	if(cn === '')
            cn = 'n';
        var ci = client_id;
        if(ci === '')
            ci = 'n';
        var cs = client_secret;
        if(cs === '')
            cs = 'n';
        var at = access_token;
        if(at === '')
            at = 'n';
        var rt = refresh_token;
        if(rt === '')
            rt = 'n';
        var returnStr = cn + ' ' + ci + ' ' + cs + ' ' + at + ' ' + rt;
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    glClear: function(mask)
    {
        if (mask == 0x00004000)
        {
            var v = GLctx.getParameter(GLctx.COLOR_WRITEMASK);
            if (!v[0] && !v[1] && !v[2] && v[3])
                // We are trying to clear alpha only -- skip.
                return;
        }
        GLctx.clear(mask);
    },
};
mergeInto(LibraryManager.library, plugin);