const { dirname, join } = require('path')

const CopyPlugin = require('copy-webpack-plugin')
const MiniCssExtractPlugin = require('mini-css-extract-plugin')
const TerserPlugin = require('terser-webpack-plugin')

// Module resolution
const frontendPath = dirname(require.resolve('govuk-frontend/package.json'))

// Build paths
const srcPath = join(__dirname, '')
const destPath = join(__dirname, 'wwwroot/dist')

/**
 * @type {() => import('webpack-dev-server').WebpackConfiguration}
 */
module.exports = ({ WEBPACK_SERVE }, { mode }) => ({
  devServer: {
    watchFiles: {
      paths: ['**/*.html']
    }
  },

  devtool: WEBPACK_SERVE ? 'inline-source-map' : 'source-map',

  entry: {
    site: [
      join(srcPath, 'wwwroot/js/site.js'),
      join(srcPath, 'wwwroot/css/site.scss')
    ]
  },

  module: {
    rules: [
      {
        test: /\.(cjs|js|mjs)$/,
        exclude: /node_modules/,
        use: {
          loader: 'babel-loader',
          options: {
            rootMode: 'upward'
          }
        }
      },
      {
        test: /\.scss$/,
        use: [
          MiniCssExtractPlugin.loader,
          {
            loader: 'css-loader',
            options: {
              // Allow sass-loader to process CSS @import first
              // before we use css-loader to extract `url()` etc
              importLoaders: 2
            }
          },
          'postcss-loader',
          {
            loader: 'sass-loader',
            options: {
              sassOptions: {
                includePaths: [`${frontendPath}/src`],
                quietDeps: true
              }
            }
          }
        ]
      },
      {
        test: /\.(woff|woff2)$/,
        type: 'asset/resource',
        generator: {
          filename: 'assets/fonts/[name][ext]'
        }
      },
      {
        test: /\.(png|svg)$/,
        type: 'asset/resource',
        generator: {
          filename: 'assets/images/[name][ext]'
        }
      }
    ]
  },

  optimization: {
    minimize: mode === 'production',
    minimizer: [
      new TerserPlugin({
        extractComments: true,
        terserOptions: {
          format: { comments: false },

          // Compatibility workarounds
          safari10: true
        }
      })
    ]
  },

  output: {
    clean: true,
    filename: 'js/[name].min.js',
    iife: true,
    path: destPath,
    publicPath: '/'
  },

  plugins: [
    new MiniCssExtractPlugin({
      filename: 'css/[name].min.css'
    })
  ],

  resolve: {
    alias: {
      '/assets/': join(frontendPath, './src/govuk/assets/')
    }
  },

  stats: {
    preset: 'minimal',
    errorDetails: true
  },

  target: ['web', 'es2015']
})