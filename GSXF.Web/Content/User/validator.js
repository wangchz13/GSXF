$.extend($.fn.validatebox.defaults.rules, {
    /*必须和某个字段相等*/
    equalTo: { validator: function (value, param) { return $(param[0]).val() == value; }, message: '字段不匹配' },
    idcard: {
        validator: function (value) {
            return /^(\d{18}|\d{17}X)$/.test(value);
        },
        message:'身份证号码格式不正确'
    },
    mobilephone:{
        validator:function(value){
            return value.length == 11;
        },
        message:'手机号格式不正确'
    },
    password: {
        validator: function (value) {
            return value.length > 5 && value.length < 21;
        },
        message:'密码必须在6~20个字符之间'
    }
});