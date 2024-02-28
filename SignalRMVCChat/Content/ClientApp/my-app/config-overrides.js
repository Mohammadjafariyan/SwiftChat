const webpack = require('webpack');

const { override, addWebpackPlugin } = require('customize-cra');
const ReactRefreshPlugin = require('@pmmmwh/react-refresh-webpack-plugin');


module.exports = function override(config) {
    const fallback = config.resolve.fallback || {};

    config.devServer = {
        ...config.devServer,
        hot: true,
        watchContentBase: true,
    };

    config.plugins.push(new webpack.HotModuleReplacementPlugin());
    config.plugins.push( new webpack.DefinePlugin({
        'process.env.NODE_ENV': JSON.stringify('development'),
    }));
    Object.assign(fallback, {
        "crypto":false,
        "stream":false,
        "assert":false,
        "http":false,
        "https":false,
        "os":false,
        "querystring":false,
        "path":false,
        "path-browserify":false,
        "vm":false,
        "querystring":false,
        "zlib":false,
        "net":false,
        "tls":false,
        "fs":false,
        "url":false
        
      /*  "crypto": require.resolve("crypto-browserify"),
        "stream": require.resolve("stream-browserify"),
        "assert": require.resolve("assert"),
        "http": require.resolve("stream-http"),
        "https": require.resolve("https-browserify"),
        "os": require.resolve("os-browserify"),
        "querystring": require.resolve("querystring-es3"),
        "path": require.resolve("path-browserify"),
        "path-browserify": require.resolve("path-browserify"),
        "vm": require.resolve("vm-browserify"),
        "querystring": require.resolve("querystring-es3"),
        "zlib": require.resolve("browserify-zlib"),
        "url": require.resolve("url")*/
    })

    config.resolve.fallback = fallback;
    config.plugins = (config.plugins || []).concat([
        new webpack.ProvidePlugin({
            process: 'process/browser',
            Buffer: ['buffer', 'Buffer']
        })
    ])

    addWebpackPlugin(new ReactRefreshPlugin())

    return config;
}