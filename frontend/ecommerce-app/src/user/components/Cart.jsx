import axios from 'axios'
import React, { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import giphy from '../../giphyy.gif'
import NavBar from '../pages/NavBar'

function Cart() {
  const [cartItems, setCartItems] = useState([])
  const navigate = useNavigate()

  useEffect(() => {
    loadCartItems()
  }, [])

  const loadCartItems = async () => {
    try {
      const userId = sessionStorage.getItem('userID')
      const response = await axios.get(
        `https://localhost:7253/api/Cart/${userId}`
      )
      setCartItems(response.data)
    } catch (error) {
      console.error('Error loading cart items:', error)
      toast.error('Error loading cart items')
    }
  }

  const removeFromCart = async (id) => {
    try {
      await axios.delete(`https://localhost:7253/api/Cart/${id}`)
      setCartItems(cartItems.filter((item) => item.id !== id))
      toast.error('Product removed from cart.')
    } catch (error) {
      console.error('Error removing product from cart:', error)
      toast.error('Error removing product from cart.')
    }
  }

  const increaseQuantity = (id) => {
    setCartItems((prevItems) =>
      prevItems.map((item) => {
        if (item.id === id) {
          return { ...item, quantity: item.quantity + 1 }
        }
        return item
      })
    )
  }

  const decreaseQuantity = (id) => {
    setCartItems((prevItems) =>
      prevItems.map((item) => {
        if (item.id === id && item.quantity > 1) {
          return { ...item, quantity: item.quantity - 1 }
        }
        return item
      })
    )
  }

  const handleOrderNow = async (productID, id, quantity) => {
    try {
      const userId = sessionStorage.getItem('userID')
      const orderNo = generateOrderNo()
      console.log('Product ID : ', productID)

      // Check if productId is valid (not 0)
      if (productID !== 0) {
        await axios.post(`https://localhost:7253/api/Order`, {
          userId,
          orderNo,
          productID,
          quantity,
          orderStatus: 'Pending',
          products: null,
        })

        setCartItems([])
        navigate(`/order`)
        await axios.delete(`https://localhost:7253/api/Cart/${id}`)
        toast.success('Order placed successfully!')
      } else {
        toast.error('Please select a valid product.')
      }
    } catch (error) {
      console.error('Error placing order:', error)
      toast.error('Error placing order.')
    }
  }

  const generateOrderNo = () => {
    return Math.floor(100000 + Math.random() * 900000).toString()
  }

  const formatCurrency = (value) => {
    return new Intl.NumberFormat('en-IN', {
      maximumFractionDigits: 2,
      minimumFractionDigits: 2,
    }).format(value)
  }

  return (
    <div>
      <NavBar />
      {cartItems.length === 0 ? (
        <div className='container-fluid' style={{ minHeight: '80vh' }}>
          <div
            className='row justify-content-center align-items-center'
            style={{ minHeight: 'inherit' }}
          >
            <div className='col-12 text-center'>
              <img src={giphy} alt='Empty cart emo' style={{ width: '10vw' }} />
              <h5>Your Shoppify Cart is empty.</h5>
            </div>
          </div>
        </div>
      ) : (
        <div className='row row-cols-1 row-cols-md-2 mx-md-1 mt-3'>
          {cartItems.map((item) => (
            <div key={item.id} className='col-md-6 mb-4'>
              <div className='card'>
                <div className='row no-gutters'>
                  <div className='col-md-5'>
                    <img
                      src={item.products.imageUrl}
                      className='card-img'
                      alt={item.products.name}
                      style={{
                        height: '100%',
                        width: '100%',
                        objectFit: 'contain',
                      }}
                    />
                  </div>
                  <div className='col-md-7'>
                    <div className='card-body'>
                      <h5 className='card-title' title={item.products.name}>
                        {item.products.name.length > 30
                          ? `${item.products.name.substring(0, 23)}... `
                          : item.products.name}
                      </h5>
                      <p className='card-text'>
                        <span style={{ fontSize: '20px' }}>₹ </span>
                        <strong style={{ fontSize: '2rem' }}>
                          {formatCurrency(item.products.unitPrice)}
                        </strong>{' '}
                        <br />
                        <b>Manufacturer</b>: {item.products.manufacturer} <br />
                        <b>Discount</b>: {item.products.discount}% <br />
                        <b>Quantity</b>: {item.quantity} <br />
                      </p>
                      <div className='row mt-3'>
                        <div className='col-md-2'>
                          <button
                            className='btn btn-primary btn-block'
                            onClick={() => decreaseQuantity(item.id)}
                          >
                            -
                          </button>
                        </div>
                        <div className='col-md-2'>
                          <button
                            className='btn btn-secondary btn-block'
                            disabled
                          >
                            {item.quantity}
                          </button>
                        </div>
                        <div className='col-md-2'>
                          <button
                            className='btn btn-primary btn-block'
                            onClick={() => increaseQuantity(item.id)}
                          >
                            +
                          </button>
                        </div>
                      </div>
                      <div className='row mt-3'>
                        <div className='col-md-6'>
                          <button
                            className='btn btn-primary btn-block'
                            onClick={() =>
                              handleOrderNow(
                                item.productID,
                                item.id,
                                item.quantity
                              )
                            }
                          >
                            Order Now
                          </button>
                        </div>
                        <div className='col-md-6'>
                          <button
                            className='btn btn-danger btn-block'
                            onClick={() => removeFromCart(item.id)}
                          >
                            Remove
                          </button>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

export default Cart
