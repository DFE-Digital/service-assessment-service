const path = require("path");

module.exports = {
  entry: {
    site: "./wwwroot/js/site.js",
  },
  output: {
    filename: "[name].entry.js",
    path: path.resolve(__dirname, ".", "wwwroot", "dist"),
  },
  devtool: "source-map",
  mode: "development",
  module: {
    rules: [
      {
        test: /\.(scss|css)$/,
        use: ["style-loader", "css-loader", "sass-loader"],
      },
      {
        test: /\.(eot|woff(2)?|ttf|otf|svg)$/i,
        type: "asset",
      },
    ],
  },
};
