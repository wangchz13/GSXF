$(function () {
    var tabs = $('#tabs').tabs({
        fit: true,
        border: false,
        tools: [{
            iconCls: 'icon-house',
            handler: function () {
                tabs.tabs('select', 0);
            }
        }, {
            iconCls: 'icon-reload',
            handler: function () {
                var refresh_tab = $('#tabs').tabs('getSelected');
                var refresh_iframe = refresh_tab.find('iframe')[0];
                refresh_iframe.contentWindow.location.href = refresh_iframe.src;
            }
        }, {
            iconCls: 'icon-no',
            handler: function () {
                var index = tabs.tabs('getTabIndex', tabs.tabs('getSelected'));
                var tab = tabs.tabs('getTab', index);
                if (tab.panel('options').closable) {
                    tabs.tabs('close', index);
                }
            }
        }]
    });

    $('#menu').tree({
        animate: true,
        onSelect: function (node) {
            addTab(node.text, node.attributes.url);
        },
    });

    var node = $('#menu').tree('find', 'home');
    $('#menu').tree('select', node.target);

    var tab = $('#tabs').tabs('getSelected');
    $('#tabs').tabs('update', {
        tab: tab,
        options: {
            closable: false
        }
    });
});

function addTab(title, url) {

    if ($('#tabs').tabs('exists', title)) {
        $('#tabs').tabs('select', title);
    } else {
        var content = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
        $('#tabs').tabs('add', {
            title: title,
            content: content,
            closable: true
        });
    }
}
function logout() {
    $.messager.confirm('提示', '确定要退出吗?', function (r) {
        if (r) {
            $.messager.progress({
                text: '正在退出中...'
            });
            window.location.href = '/Data/LoginOut';
        }
    });
}