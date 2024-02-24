module.exports = {
    // Other webpack configuration options...
    module: {
        rules: [
            // Other rules...
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['@babel/preset-react']
                    }
                }
            },
            // Other rules...
        ]
    }
};