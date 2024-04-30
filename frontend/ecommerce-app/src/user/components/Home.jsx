import axios from 'axios'
import React, { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import NavBar from '../pages/NavBar'

function Home() {
  const [products, setProducts] = useState([])
  const [searchTerm, setSearchTerm] = useState('')

  useEffect(() => {
    loadProducts()
  }, [])

  const loadProducts = async () => {
    try {
      const result = await axios.get('https://localhost:7253/api/Product')
      if (result.status === 200) {
        setProducts(result.data)
      } else {
        toast.error(result.error)
      }
    } catch (error) {
      console.error('Error loading products:', error)
      toast.error('Error loading products')
    }
  }

  const getStatusColor = (status) => {
    return status === 0 ? 'text-success' : 'text-danger'
  }

  const formatCurrency = (value) => {
    return new Intl.NumberFormat('en-IN', {
      maximumFractionDigits: 2,
      minimumFractionDigits: 2,
    }).format(value)
  }

  const addToCart = async (productId) => {
    const userId = sessionStorage.getItem('userID')
    try {
      await axios.post('https://localhost:7253/api/Cart', {
        userId,
        productId,
        products: null,
      })
      toast.success('Product added to cart.')
    } catch (error) {
      toast.error('Error adding product to cart.')
    }
  }

  const filteredProducts = products.filter(
    (product) =>
      product.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      product.manufacturer.toLowerCase().includes(searchTerm.toLowerCase())
  )

  const handleSearchTermChange = (event) => {
    setSearchTerm(event.target.value)
  }

  return (
    <div>
      <NavBar />
      <div className='searchBox col-md-6 mt-4 mx-auto'>
        <div className='input-group'>
          <input
            type='search'
            placeholder='Search by name or manufacturer'
            value={searchTerm}
            onChange={handleSearchTermChange}
            className='form-control border-end-0 rounded-start searchInputBox'
          />
          <button
            className='btn btn-outline-secondary border-start-0 rounded-end p-2'
            type='button'
          >
            🔍
          </button>
        </div>
      </div>
      <div className='row row-cols-1 row-cols-md-2 mx-md-1'>
        {filteredProducts.map((product) => (
          <div key={product.id} className='col mb-4'>
            <div className='card'>
              <div className='row no-gutters'>
                <div className='col-md-5'>
                  <img
                    src={product.imageUrl}
                    className='card-img'
                    alt={product.name}
                    style={{
                      height: '100%',
                      width: '100%',
                      objectFit: 'contain',
                    }}
                  />
                </div>
                <div className='col-md-7'>
                  <div className='card-body'>
                    <h5 className='card-title' title={product.name}>
                      <strong>
                        {product.name.length > 22
                          ? `${product.name.substring(0, 25)}...`
                          : product.name}
                      </strong>
                    </h5>

                    <p className='card-text'>
                      <span style={{ fontSize: '20px' }}>₹ </span>
                      <strong style={{ fontSize: '2rem' }}>
                        {formatCurrency(product.unitPrice)}
                      </strong>{' '}
                      <br />
                      M.R.P : ₹{' '}
                      <span style={{ textDecoration: 'line-through' }}>
                        {formatCurrency(product.mrp)}
                      </span>
                      <br />
                      <b>Manufacturer</b> : {product.manufacturer}
                      <br />
                      <b>Discount</b> : -{product.discount}%<br />
                      <b>Status</b> :{' '}
                      <span className={getStatusColor(product.status)}>
                        <strong>
                          {product.status === 0 ? 'Available' : 'Out of stock'}
                        </strong>
                      </span>
                    </p>
                    {product.status === 0 ? (
                      <button
                        className='btn btn-primary'
                        onClick={() => addToCart(product.id)}
                      >
                        Add to Cart
                      </button>
                    ) : (
                      <button
                        style={{ textDecoration: 'line-through' }}
                        className='btn btn-primary'
                        disabled
                      >
                        Add to Cart
                      </button>
                    )}
                  </div>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}

export default Home
