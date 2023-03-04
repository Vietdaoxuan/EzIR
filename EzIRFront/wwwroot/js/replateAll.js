(function () {
    this.initForm = function () {
        String.prototype.trim = function () { return this.replace(/^\s+|\s+$/gi, ''); };
        String.prototype.ltrim = function () { return this.replace(/^\s+/, ''); };
        String.prototype.rtrim = function () { return this.replace(/\s+$/, ''); };
        String.prototype.fulltrim = function () { return this.replace(/(?:(?:^|\n)\s+|\s+(?:$|\n))/gi, '').replace(/\s+/gi, ' '); };
        String.prototype.left = function (n) { if (n <= 0) return ""; else if (n > this.length) return this; else return this.substring(0, n); }
        String.prototype.right = function (n) { if (n <= 0) return ""; else if (n > this.length) return str; else { var iLen = this.length; return this.substring(iLen, iLen - n); } }
        
        String.prototype.ReplaceAll = function (s1, s2) {
            var regex = new RegExp(s1, "gi");
            if (typeof this['replace'] === 'undefined') {
                return this;
            }
            var result = this.replace(regex, s2);
            return result;
        }
    }

    // contructor => auto exec
    this.initForm();

    // global instance
    window.g_classReplateAll = this;
})();
