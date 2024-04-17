import {Flex, VStack, Heading, Box, FormControl, Input, Stack, InputGroup, InputLeftElement, Button, FormLabel, HStack, Text} from "@chakra-ui/react"
import {FiMail } from "react-icons/fi"
import React, { useState } from 'react'
import { useForm } from 'react-hook-form';
import { useToast } from '@chakra-ui/react'
import { checkOtp, getOtp } from "../service/OtpService";
import { CountdownCircleTimer } from 'react-countdown-circle-timer'
import { TbPasswordUser } from "react-icons/tb";

const Login = () => {
    const {
        handleSubmit,
        register,
        formState: { errors },
      } = useForm();

      const {
        handleSubmit : handleSubmitOtp,
        register: registerOtp,
        setValue: setValueOtp,
        formState: { errors: errorsOtp },
      } = useForm();

    const textColor = 'gray.200';
    const inputBorderFocusColor = 'blue.900';
    const toast = useToast();
    const toastIdRef = React.useRef()

    const [otpGenerate, setOtpGenerated] = useState(false)
    const [email, setEmail] = useState("")

    const renderTime = ({ remainingTime }) => {
        if (remainingTime === 0) {
          return <Text>Too lale</Text>;
        }
        return (
            <VStack>
                <Text>{remainingTime}</Text>
            </VStack>
          );
    }

    const getDuration = date => {
        
        console.log(Date.now())
        var dateParsed = Date.parse(date)
        console.log(dateParsed)
        var dif = Math.round(Math.round(dateParsed - Date.now())/1000)
        console.log( dif )
        return dif
    }

    const closeToastTimmer = () => {
        if (toastIdRef.current) {
            toast.close(toastIdRef.current)
        }
    }

    const back = () =>{
        setOtpGenerated(false)
        setEmail("")
        closeToastTimmer()
        setValueOtp("otp", "")
    }

    const onSubmitEmail = data => {
        getOtp(data.email)
        .then(result=>{
            var duration = getDuration(result.data.expiry)
            setOtpGenerated(true)
            setEmail(data.email)
            toastIdRef.current = toast({
                position: 'top-right',
                duration:duration*1000,
                render: () => (
                    <HStack >
                        <CountdownCircleTimer
                            isPlaying
                            duration={duration}
                            colors={"#004777"}
                            size={60}
                            strokeWidth={5}
                            >
                            {renderTime}
                        </CountdownCircleTimer>
                        <Text fontSize="xl">OTP: {result.data.code}</Text>
                    </HStack>
                )
            });
        })
        .catch(error => {
            if (error) {
              if (error.code) { //switch
                toast({
                  title: error.code,
                  status: 'Email is not valid',
                  duration: 3000,
                  isClosable: true,
                });
              } else {
                toast({
                  title: 'An Error Occurred',
                  status: 'error',
                  duration: 3000,
                  isClosable: true,
                });
              }
            }
        });
    }

    const onSubmitOtp = data => {
        checkOtp(email, data.otp)
        .then(result=>{
            console.log(result)
            toast({
                title: 'You are logged in',
                status: "success",
                duration: 10000,
                isClosable: true,
              });
            back();
        })
        .catch(error => {
            if (error) {
              if (error.code) {
                toast({
                  title: "OTP is not valid",
                  status: 'error',
                  duration: 3000,
                  isClosable: true,
                });
              } else {
                toast({
                  title: 'An Error Occurred',
                  status: 'error',
                  duration: 3000,
                  isClosable: true,
                });
              }
            }
        });
    }

    return (
    <HStack w="100vw" h="100vh" spacing={0}>
        <Box w="45%" h="100%" bg="blue.900">
            <Flex h="100%" justifyContent="center" alignItems="center">
                <VStack alignItems="start">
                <Heading fontSize="4xl" color={textColor}>
                    Login Page
                </Heading>
                </VStack>
            </Flex>
        </Box>
        <Box w="55%" h="100%">
            <Flex h="100%" justifyContent="center" alignItems="center">
                <VStack alignItems="start" spacing={5} w="100%" padding={20}>
                    <Heading fontSize="3xl" color={inputBorderFocusColor}>
                    Log In
                    </Heading>
                    <HStack w="100%">
                        <FormControl isInvalid={errors.email}>
                            <InputGroup>
                                <InputLeftElement pointerEvents="none">
                                    <FiMail />
                                </InputLeftElement>
                                    <Input
                                        isDisabled = {otpGenerate}
                                        placeholder='Email'
                                        focusBorderColor={inputBorderFocusColor}
                                        {...register('email', { required: true})}
                                    />
                            </InputGroup>
                        </FormControl>
                        <Button
                            isDisabled = {otpGenerate}
                            minW={'200px'}
                            alignSelf="center"
                            color="white"
                            bgColor={inputBorderFocusColor}
                            _hover={{ backgroundColor: 'var(--chakra-colors-blue-700)' }}
                            variant="solid"
                            onClick={handleSubmit(onSubmitEmail)}
                        >
                            Get OTP
                        </Button>
                    </HStack>
                    {
                        otpGenerate?
                        <VStack w="100%">
                            <HStack w="100%">
                                <FormControl isInvalid={errorsOtp.otp}>
                                    <InputGroup>
                                    <InputLeftElement pointerEvents="none">
                                            <TbPasswordUser />
                                    </InputLeftElement>
                                    <Input
                                        placeholder='OTP'
                                        focusBorderColor={inputBorderFocusColor}
                                        {...registerOtp('otp', { required: true })} />
                                        </InputGroup>
                                    </FormControl>
                                    <Button
                                        minW={'200px'}
                                        alignSelf="center"
                                        color="white"
                                        bgColor={inputBorderFocusColor}
                                        _hover={{ backgroundColor: 'var(--chakra-colors-blue-700)' }}
                                        variant="solid"
                                        onClick={handleSubmitOtp(onSubmitOtp)}
                                    >
                                    Log In
                                    </Button>
                            </HStack>
                            <Button
                                minW={'200px'}
                                alignSelf="left"
                                color="white"
                                bgColor={inputBorderFocusColor}
                                _hover={{ backgroundColor: 'var(--chakra-colors-blue-700)' }}
                                variant="solid"
                                onClick={back}
                            >
                                Back
                            </Button>
                        </VStack>
                        
                        :<></>
                    }
                </VStack>
            </Flex>
        </Box>
    </HStack>
  )
}

export default Login